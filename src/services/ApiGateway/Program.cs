using Shared.Middlewares;
// using ServiceDefaults; // <-- УДАЛИТЕ ИЛИ ЗАКОММЕНТИРУЙТЕ ЭТУ СТРОКУ

var builder = WebApplication.CreateBuilder(args);

// Метод AddServiceDefaults находится в пространстве имен Microsoft.Extensions.Hosting,
// которое обычно подключено автоматически. Если возникнет ошибка здесь, добавьте:
// using Microsoft.Extensions.Hosting; 
builder.AddServiceDefaults();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseMiddleware<CorrelationIdMiddleware>();

app.MapReverseProxy();

app.Run();