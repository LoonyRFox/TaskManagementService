
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using TaskManagementServiceLoging.Domain.Uptrace;
using TaskManagementServiceLoging.Infrastructure.Confuguration;
using TaskManagementServiceLoging.WebApi.Infrastructure.Extensions;
using Uptrace.OpenTelemetry;

namespace TaskManagementServiceLoging.WebApi.Infrastructure.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        var uptraceOptions = configuration.GetSection(UptraceConfiguration.Section)
            .Get<UptraceConfiguration>()!;

        if (string.IsNullOrEmpty(uptraceOptions.Dsn))
        {
            return services;
        }

        // рекомендуется создать 1 ActivitySource / Meter
        services.AddSingleton<OpenTelemetryHolder>();

        var openTelemetryBuilder =
            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService(uptraceOptions.ServiceName,
                        uptraceOptions.ServiceNamespace)
                    .AddAttributes([
                        new KeyValuePair<string, object>("deployment.environment", uptraceOptions.Environment.ToLower())
                    ]))
                .WithTracing(tracing => tracing
                    .AddSource(OpenTelemetryHolder.ActivitySourceName)
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddUptrace(uptraceOptions.Dsn));

        if (uptraceOptions.UseMetrics)
        {
            openTelemetryBuilder
                .WithMetrics(metric => metric
                        .AddMeter(OpenTelemetryHolder.MeterName)
                        .AddHttpClientInstrumentation()
                        .AddAspNetCoreInstrumentation()
                        .AddUptrace(uptraceOptions.Dsn)
                );
        }

        return services;
    }
}