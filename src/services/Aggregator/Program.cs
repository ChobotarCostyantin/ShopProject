using Aggregator.DTOs;
using Aggregator.Services;
using Catalog.BLL.DTOs.Products.Responces;
using Orders.BLL.Features.Orders.DTOs.Responces;
using Shared.DTOs;
using Shared.Http;
using Shared.Middlewares;
using SocialAndReviews.Application.Reviews.DTOs.Responces;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://http://orders-api/health"), name: "orders-api", tags: ["api"])
    .AddUrlGroup(new Uri("http://catalog-api/health"), name: "catalog-api", tags: ["api"])
    .AddUrlGroup(new Uri("http://social-and-reviews-api/health"), name: "social-and-reviews-api", tags: ["api"]);

builder.AddServiceDefaults();

// 1. Потрібно для доступу до HttpContext у DelegatingHandler
builder.Services.AddHttpContextAccessor();

// 2. Реєструємо хендлер
builder.Services.AddTransient<CorrelationIdDelegatingHandler>();

// 3. Реєструємо Typed Clients з прив'язкою до handler та BaseAddress
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
    CancellationToken ct) =>
{
    var productTask = catalogClient.GetProductByIdAsync(productId, ct);
    var reviewsTask = socialClient.GetReviewsByProductIdAsync(productId, ct);
    var ordersTask = ordersClient.GetOrdersByProductIdAsync(productId, ct);

    await Task.WhenAll(productTask, reviewsTask, ordersTask);

    var product = productTask.Result;
    var reviewsPagination = reviewsTask.Result;
    var ordersPagination = ordersTask.Result;

    if (product is null)
        return Results.NotFound();

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