namespace TaskManagementServiceLoging.Infrastructure.Confuguration;

public class DatabaseOptions
{
    public const string Section = "DatabaseOptions";

    public string Schema { get; set; } = "public";
    public string Name { get; set; } = default!;
    public string Host { get; set; } = default!;
    public int Port { get; set; } = 5432;
    public string User { get; set; } = default!;
    public string Password { get; set; } = default!;
    public int CommandTimeout { get; set; } = 300;
    public int ConnectionIdLifetime { get; set; } = 300;
    public bool Pooling { get; set; } = false;
    public int KeepAlive { get; set; } = 30;
    public bool TcpKeepAlive { get; set; } = true;
    public bool ShowSqlQuery { get; set; } = false;
}