using System.Text.Json.Serialization;

namespace TaskManagementService.Application.DTOs.Info;

public class HealthCheckResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; } = "ok";
    [JsonPropertyName("message")]
    public string? Message { get; set; } = null;
}