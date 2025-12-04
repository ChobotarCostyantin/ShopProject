using Aggregator.DTOs;
using Aggregator.Services;
using Catalog.BLL.DTOs.Products.Responces;
using Orders.BLL.Features.Orders.DTOs.Responces;
using Shared.DTOs;
using Shared.Http;
using Shared.Middlewares;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://orders-api/health"), name: "orders-api", tags: ["api"])
    .AddUrlGroup(new Uri("http://catalog-api/health"), name: "catalog-api", tags: ["api"])
    .AddUrlGroup(new Uri("http://social-and-reviews-api/health"), name: "social-and-reviews-api", tags: ["api"]);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

// 2. Реєструємо хендлер
builder.Services.AddTransient<CorrelationIdDelegatingHandler>();

builder.Services.AddHttpClient<CatalogClient>(client =>
    client.BaseAddress = new Uri("http://catalog-api"))
    .AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

builder.Services.AddHttpClient<OrdersClient>(client =>
    client.BaseAddress = new Uri("http://orders-api"))
    .AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

builder.Services.AddHttpClient<SocialAndReviewsClient>(client =>
    client.BaseAddress = new Uri("http://social-and-reviews-api"))
    .AddHttpMessageHandler<CorrelationIdDelegatingHandler>();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseMiddleware<CorrelationIdMiddleware>();

app.MapHealthChecks("/health-check", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

app.MapGet("/api/aggregator/full-product-data/{productId}", async (
    CatalogClient catalogClient,
    SocialAndReviewsClient socialClient,
    OrdersClient ordersClient,
    Guid productId,
    ILogger<Program> logger,
    CancellationToken ct) =>
{
    async Task<T?> ExecuteSafelyAsync<T>(Func<Task<T?>> action, string serviceName)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Не вдалося отримати дані з {ServiceName}", serviceName);
            return default;
        }
    }

    var productTask = ExecuteSafelyAsync(() => catalogClient.GetProductByIdAsync(productId, ct), "Catalog");
    var reviewsTask = ExecuteSafelyAsync(() => socialClient.GetReviewsByProductIdAsync(productId, ct), "SocialAndReviews");
    var ordersTask = ExecuteSafelyAsync(() => ordersClient.GetOrdersByProductIdAsync(productId, ct), "Orders");

    await Task.WhenAll(productTask, reviewsTask, ordersTask);

    var product = productTask.Result;
    var reviewsPagination = reviewsTask.Result;
    var ordersPagination = ordersTask.Result;

    if (product is null)
    {
        logger.LogWarning("Для ID відсутні дані продукту: {ProductId}", productId);
        return Results.NotFound();
    }

    var reviewArray = reviewsPagination?.Entities ?? [];
    var orderArray = ordersPagination?.Entities ?? [];

    var averageRating = reviewArray.Length > 0
        ? reviewArray.Average(r => r.Rating)
        : 0;

    var result = new FullProductDataDto(
        Product: product,
        Reviews: reviewArray,
        AverageRating: averageRating,
        Orders: orderArray
    );

    return Results.Ok(result);
});

app.Run();