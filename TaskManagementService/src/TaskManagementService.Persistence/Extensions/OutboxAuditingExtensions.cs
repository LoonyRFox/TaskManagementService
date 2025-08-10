using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaskManagementService.Domain.Entity;
using TaskManagementService.Domain.Events;
using TaskManagementService.Domain.Messages;


namespace TaskManagementService.Persistence.Extensions;

public static class OutboxAuditingExtensions
{
    public static void AddDomainEventsToOutbox(this DbContext context)
    {
        var entries = context.ChangeTracker
            .Entries<TaskItem>()
            .Where(e => e.State == EntityState.Added ||
                        e.State == EntityState.Modified ||
                        e.State == EntityState.Deleted)
            .ToList(); // <- материализация в список

        foreach (var entry in entries)
        {
            object @event = entry.State switch
            {
                EntityState.Added => new TaskCreatedEvent(
                    ((TaskItem)entry.Entity).Id,
                    ((TaskItem)entry.Entity).Title,
                    ((TaskItem)entry.Entity).Description
                ),
                EntityState.Modified => new TaskUpdatedEvent(
                    ((TaskItem)entry.Entity).Id,
                    ((TaskItem)entry.Entity).Title,
                    ((TaskItem)entry.Entity).Description
                ),
                EntityState.Deleted => new TaskDeletedEvent(
                    ((TaskItem)entry.Entity).Id
                ),
                _ => null
            };

            if (@event != null)
            {
                var outbox = new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredOnUtc = DateTime.UtcNow,
                    Type = @event.GetType().AssemblyQualifiedName!,
                    Payload = JsonSerializer.Serialize(@event)
                };

                context.Set<OutboxMessage>().Add(outbox);
            }
        }
    }
}