using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Shared.Exceptions;
using SocialAndReviews.Domain.Exceptions;

namespace Shared.ErrorHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception,
            "Unhandled exception occurred. Path: {RequestPath}, CorrelationId: {CorrelationId}",
            httpContext.Request.Path,
            httpContext.Items["X-Correlation-Id"]);
        
        var problemDetails = CreateProblemDetails(exception, httpContext);

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = problemDetails.Status ?? (int)HttpStatusCode.InternalServerError;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, options), cancellationToken: cancellationToken);

        return true;
    }

    private ProblemDetails CreateProblemDetails(Exception exception, HttpContext context)
    {
        return exception switch
        {
            // 404: Not Found (Специфічна обробка для EntityNotFound)
            EntityNotFoundException notFoundEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Resource not found",
                Detail = notFoundEx.Message,
                Status = (int)HttpStatusCode.NotFound,
                Instance = context.Request.Path
            },

            // 409: Conflict (Загальні конфлікти)
            ConflictException conflictEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/409",
                Title = "Resource conflict",
                Detail = conflictEx.Message,
                Status = (int)HttpStatusCode.Conflict,
                Instance = context.Request.Path
            },

            // 409: Conflict (MongoDB Duplicate Key)
            // Код помилки 11000 в MongoDB означає Duplicate Key Error
            MongoWriteException mongoWriteEx when mongoWriteEx.WriteError.Category == ServerErrorCategory.DuplicateKey => new ProblemDetails
            {
                Type = "https://httpstatuses.com/409",
                Title = "Duplicate key conflict",
                Detail = "A record with the same unique key already exists.",
                Status = (int)HttpStatusCode.Conflict,
                Instance = context.Request.Path
            },

            // 400: Bad Request (MongoDB Write Error - інші помилки запису, наприклад валідація схеми)
            MongoWriteException mongoWriteEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "Database write error",
                Detail = _env.IsDevelopment() ? mongoWriteEx.Message : "Failed to write data to the database due to validation rules.",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = context.Request.Path
            },

            // 503: Service Unavailable (MongoDB Connection Issues)
            MongoConnectionException => new ProblemDetails
            {
                Type = "https://httpstatuses.com/503",
                Title = "Service Unavailable",
                Detail = "Database connection is currently unavailable.",
                Status = (int)HttpStatusCode.ServiceUnavailable,
                Instance = context.Request.Path
            },
            
            // 503: Service Unavailable (Timeouts)
            TimeoutException => new ProblemDetails
            {
                Type = "https://httpstatuses.com/503",
                Title = "Service Timeout",
                Detail = " The operation timed out.",
                Status = (int)HttpStatusCode.ServiceUnavailable,
                Instance = context.Request.Path
            },

            // 400: Bad Request (Domain Logic Violations)
            DomainException domainEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "Domain validation failed",
                Detail = domainEx.Message,
                Status = (int)HttpStatusCode.BadRequest,
                Instance = context.Request.Path
            },

            // 400: Validation (FluentValidation)
            ValidationException validationEx => new ValidationProblemDetails(
                validationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            )
            {
                Type = "https://httpstatuses.com/400",
                Title = "Validation failed",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = context.Request.Path
            },

            // 503: Database Unavailable (Загальна інфраструктура)
            DatabaseUnavailableException => new ProblemDetails
            {
                Type = "https://httpstatuses.com/503",
                Title = "Database unavailable",
                Detail = "The service is currently unavailable. Please try again later.",
                Status = (int)HttpStatusCode.ServiceUnavailable,
                Instance = context.Request.Path
            },

            // 500: Unhandled (Все інше)
            _ => new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "An unexpected error occurred",
                Detail = _env.IsDevelopment() ? exception.ToString() : "An unexpected error occurred.",
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path
            }
        };
    }
}