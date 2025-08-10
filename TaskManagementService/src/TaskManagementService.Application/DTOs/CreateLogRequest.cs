using System.Text.Json.Serialization;

namespace TaskManagementService.Application.DTOs;

public class CreateLogRequest
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("payload")]
    public string Payload { get; set; } = null!;
    [JsonPropertyName("processedOnUtc")]
    public DateTime? ProcessedOnUtc { get; set; }
    [JsonPropertyName("error")]
    public string? Error { get; set; }
}