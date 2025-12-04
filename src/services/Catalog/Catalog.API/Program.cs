using Catalog.BLL;
using Catalog.DAL;
using Catalog.DAL.Database;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.ErrorHandling;
using Shared.Logging;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("catalogDb")!,
        name: "catalog-db",
        failureStatus: HealthStatus.Unhealthy,
        tags: ["db", "postgres"],
        timeout: TimeSpan.FromSeconds(5));

builder.AddServiceDefaults();

builder.Host.ConfigureSerilog();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();

builder.Services
    .ConfigureDataAccessLayer(builder.Configuration)
    .ConfigureBusinessLayer();

var app = builder.Build();

app.UseMiddleware<CorrelationIdMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    await app.MigrateDatabaseAsync();
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