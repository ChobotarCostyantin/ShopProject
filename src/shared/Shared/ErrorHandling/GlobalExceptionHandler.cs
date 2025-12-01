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
            // 404: Not Found (Специфічна обробка для EntityNotFound)
            EntityNotFoundException notFoundEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/404",
                Title = "Resource not found",
                Detail = notFoundEx.Message,
                Status = (int)HttpStatusCode.NotFound,
                Instance = context.Request.Path
            },

            // 409: Conflict (Явний ConflictException або порушення унікальності)
            ConflictException conflictEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/409",
                Title = "Resource conflict",
                Detail = conflictEx.Message,
                Status = (int)HttpStatusCode.Conflict,
                Instance = context.Request.Path
            },
            // Розширення логіки для DatabaseConstraintException (наприклад, duplicate key)
            DatabaseConstraintException dbEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/409", // Змінюємо на 409 або залишаємо 400 залежно від бізнес-правил
                Title = "Database constraint violation",
                Detail = dbEx.Message,
                Status = (int)HttpStatusCode.Conflict, // Часто краще Conflict для унікальних індексів
                Instance = context.Request.Path
            },

            // 503: Service Unavailable
            DatabaseUnavailableException dbEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/503",
                Title = "Database unavailable",
                Detail = "The service is currently unavailable. Please try again later.", // Безпечніше для продакшену
                Status = (int)HttpStatusCode.ServiceUnavailable,
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

            // 500: Infrastructure (Всі інші помилки інфраструктури)
            InfrastructureException infraEx => new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",
                Detail = "An internal error occurred.", // Приховуємо деталі в проді
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path
            },

            // 500: Unhandled (Все інше)
            _ => new ProblemDetails
            {
                Type = "https://httpstatuses.com/500",
                Title = "An unexpected error occurred",
                Detail = "An unexpected error occurred.", // Ніколи не показуйте exception.Message тут для продакшену
                Status = (int)HttpStatusCode.InternalServerError,
                Instance = context.Request.Path
            }
        };
    }
}