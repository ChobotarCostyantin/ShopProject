using Catalog.BLL;
using Catalog.DAL;
using Catalog.DAL.Database;
using Shared.ErrorHandling;
using Shared.Logging;
using Shared.Middlewares;

var builder = WebApplication.CreateBuilder(args);

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

app.UseExceptionHandler();

app.MapControllers();

await app.RunAsync();