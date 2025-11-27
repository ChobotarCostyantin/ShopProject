using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Exceptions;

namespace Shared.ErrorHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
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

    private static ProblemDetails CreateProblemDetails(Exception exception, HttpContext context)
    {
        return exception switch
        {
            DatabaseConstraintException dbEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/400",
                Title = "Database constraint violation",
                Detail = dbEx.Message,
                Status = (int)HttpStatusCode.BadRequest,
                Instance = context.Request.Path
            },

            DatabaseUnavailableException dbEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/503",
                Title = "Database unavailable",
                Detail = dbEx.Message,
                Status = (int)HttpStatusCode.ServiceUnavailable,
                Instance = context.Request.Path
            },

            InfrastructureException infraEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Infrastructure error",
                Detail = infraEx.Message,
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path
            },
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
                Title = "One or more validation errors occurred",
                Status = (int)HttpStatusCode.BadRequest,
                Instance = context.Request.Path
            },
            _ => new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "An unexpected error occurred",
                Detail = exception.Message,
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path
            }
        };
    }
}