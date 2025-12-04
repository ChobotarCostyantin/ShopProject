using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace Shared.Logging;

public static class LoggingConfiguration
{
    public static void ConfigureSerilog(this ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .WriteTo.Console(new CompactJsonFormatter()) 
                .WriteTo.OpenTelemetry(options =>
                {
                    options.Endpoint = context.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "http://localhost:4317";
                    options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc;
                    options.ResourceAttributes = new Dictionary<string, object>
                    {
                        ["service.name"] = context.HostingEnvironment.ApplicationName
                    };
                });

            // (Решта коду без змін...)
            var minLevel = context.Configuration["Serilog:MinimumLevel:Default"];
            if (Enum.TryParse<LogEventLevel>(minLevel, out var level))
            {
                configuration.MinimumLevel.Is(level);
            }
            else
            {
                configuration.MinimumLevel.Information();
            }

            configuration
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning);
        });
    }
}