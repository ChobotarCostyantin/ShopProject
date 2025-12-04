using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.ErrorHandling;
using Shared.Logging;
using SocialAndReviews.Application;
using SocialAndReviews.Infrastructure;
using SocialAndReviews.Infrastructure.Database;
using MongoDB.Driver;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddMongoDb(
        sp => sp.GetRequiredService<IMongoClient>(),
        name: "socialAndReviewsDb",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "mongodb" },
        timeout: TimeSpan.FromSeconds(5));


builder.AddServiceDefaults();
builder.AddMongoDBClient("socialAndReviewsDb");
builder.Host.ConfigureSerilog();

builder.Services.AddSwaggerGen();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services
    .ConfigureApplication()
    .ConfigureInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.SetupDatabase();
}

app.MapHealthChecks("/health-check", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(entry => new
            {
                name = entry.Key,
                status = entry.Value.Status.ToString(),
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

app.MapDefaultEndpoints();
app.UseExceptionHandler();

app.MapControllers();

await app.RunAsync();