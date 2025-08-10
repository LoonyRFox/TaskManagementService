using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace TaskManagementServiceLoging.Domain.Uptrace;

public class OpenTelemetryHolder: IDisposable
{
    public readonly ActivitySource ActivitySource;
    private readonly Meter Meter;

    // Все счетчики должны быть описаны тут        
    public Counter<long> TestCounter { get; }

    public static string ActivitySourceName = "finalizator-notify-service";
    public static string MeterName = "finalizator-notify-service";

    public OpenTelemetryHolder()
    {
        ActivitySource = new ActivitySource(ActivitySourceName);

        Meter = new Meter(MeterName);
        TestCounter = Meter.CreateCounter<long>("test_counter", description: "Тестовый счетчик для проверки функциональности");
    }

    public void Dispose()
    {
        ActivitySource.Dispose();
        Meter.Dispose();
    }
}