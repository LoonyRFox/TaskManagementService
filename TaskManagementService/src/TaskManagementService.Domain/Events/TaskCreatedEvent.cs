namespace TaskManagementService.Domain.Events;

public record TaskCreatedEvent(long TaskId, string Title, string Description);