using Orders.BLL;
using Orders.DAL;
using Orders.DAL.Database.Initialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Shared.ErrorHandling;
using Shared.Logging;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString("ordersDb")!,
        name: "orders-db",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "db", "postgres" },
        timeout: TimeSpan.FromSeconds(5));

builder.AddServiceDefaults();
builder.Host.ConfigureSerilog();

builder.Services
    .ConfigureBusinessLayer()
    .ConfigureDataAccessLayer(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
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

await app.MigrateDatabase();

app.MapControllers();

await app.RunAsync();