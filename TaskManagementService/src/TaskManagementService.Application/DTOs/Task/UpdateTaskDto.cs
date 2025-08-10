namespace TaskManagementService.Application.DTOs.Task;

/// <summary>DTO для обновления задачи.</summary>
public record UpdateTaskDto(string Title, string Description, TaskStatus Status);