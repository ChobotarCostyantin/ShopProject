using Orders.BLL;
using Orders.DAL;
using Orders.DAL.Database.Initialization;
using Microsoft.AspNetCore.Diagnostics;
using Shared.ErrorHandling;
using Shared.Logging;

var builder = WebApplication.CreateBuilder(args);

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

await Task.Delay(TimeSpan.FromSeconds(3));

app.MapDefaultEndpoints();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

await app.MigrateDatabase();

app.MapControllers();

await app.RunAsync();