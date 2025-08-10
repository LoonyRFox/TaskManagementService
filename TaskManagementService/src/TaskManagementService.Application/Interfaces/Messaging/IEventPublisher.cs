using TaskManagementService.Domain.Messages;

namespace TaskManagementService.Application.Interfaces.Messaging;

public interface IEventPublisher
{
    Task ProduceAsync(OutboxMessage messageDto, CancellationToken cancellationToken);
}