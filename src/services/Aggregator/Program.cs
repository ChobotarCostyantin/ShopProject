using Catalog.BLL.DTOs.Products.Responces;
using Orders.BLL.Features.Orders.DTOs.Responces;
using Shared.DTOs;
using SocialAndReviews.Application.Reviews.DTOs.Responces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddUrlGroup(new Uri("http://order-api/health"), name: "order-api", tags: ["api"])
    .AddUrlGroup(new Uri("http://catalog-api/health"), name: "catalog-api", tags: ["api"])
    .AddUrlGroup(new Uri("http://social-and-reviews-api/health"), name: "social-and-reviews-api", tags: ["api"]);

builder.AddServiceDefaults();

builder.Services.AddHttpClient("order-api", client =>
{
    client.BaseAddress = new Uri("http://order-api");
});

builder.Services.AddHttpClient("catalog-api", client =>
{
    client.BaseAddress = new Uri("http://catalog-api");
});

builder.Services.AddHttpClient("social-and-reviews-api", client =>
{
    client.BaseAddress = new Uri("http://social-and-reviews-api");
});

var app = builder.Build();
app.MapDefaultEndpoints();

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

app.MapGet("/api/full-product-data/{productId}", async (
    IHttpClientFactory httpClientFactory,
    Guid productId) =>
{
    var orderClient = httpClientFactory.CreateClient("order-api");
    var catalogClient = httpClientFactory.CreateClient("catalog-api");
    var socialAndReviewsClient = httpClientFactory.CreateClient("social-and-reviews-api");

    var product = await catalogClient.GetFromJsonAsync<ProductDto>($"api/catalog/products/{productId}");
    var orders = await orderClient.GetFromJsonAsync<List<OrderDto>>($"api/orders/orders/{productId}");
    var reviews = await socialAndReviewsClient.GetFromJsonAsync<List<ReviewDto>>($"api/social-and-reviews/reviews/{productId}");

    return Results.Ok(new {product, orders, reviews});
});

app.Run();