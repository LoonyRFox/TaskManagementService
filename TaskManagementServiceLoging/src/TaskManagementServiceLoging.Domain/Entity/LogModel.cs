namespace TaskManagementServiceLoging.Domain.Entity;

public class LogModel
{
    public long Id { get; set; }
    public Guid LogId {get; set; }
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }

    public LogModel(Guid logId, string type, string payload, DateTime? processedOnUtc, string? error)
    {
        Type = type;
        Payload = payload;
        ProcessedOnUtc = processedOnUtc;
        Error = error;
    }
}