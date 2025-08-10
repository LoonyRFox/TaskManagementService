using TaskManagementServiceLoging.Domain.Entity;

namespace TaskManagementServiceLoging.Application.DTOs.Log;

/// <summary>DTO задачи для выдачи через API.</summary>
public class LogDto
{
#pragma warning disable
    public LogDto()
    {

    }
#pragma warning restore
    public LogDto(LogModel item)
    {
        LogId = item.LogId;
        Type = item.Type;
        Payload = item.Payload;
        ProcessedOnUtc = item.ProcessedOnUtc;
        Error = item.Error;

    }
    public Guid LogId { get; set; }
    public string Type { get; set; } = null!;
    public string Payload { get; set; } = null!;
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }

}
