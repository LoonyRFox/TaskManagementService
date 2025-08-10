namespace TaskManagementServiceLoging.Infrastructure.Confuguration;

public class UptraceConfiguration
{
    public const string Section = "UptraceConfiguration";
    public string Dsn { get; set; } = default!;
    public string ServiceName { get; set; } = default!;
    public string ServiceNamespace { get; set; } = default!;
    public bool UseMetrics { get; set; }
    public string Environment { get; set; } = "not_set";
}