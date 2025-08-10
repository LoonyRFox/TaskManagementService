namespace TaskManagementService.Application.DTOs.Task;

/// <summary>DTO для создания задачи.</summary>
public record CreateTaskDto(string Title, string Description);