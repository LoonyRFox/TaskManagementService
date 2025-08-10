namespace TaskManagementService.Domain.Events;

public record TaskUpdatedEvent(long TaskId, string Title, string Description);