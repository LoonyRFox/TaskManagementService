using Microsoft.EntityFrameworkCore;
using TaskManagementService.Application.Interfaces.Messaging;
using TaskManagementService.Persistence.Context;

namespace TaskManagementService.Messaging.Services;

public class OutboxProcessor( ApplicationDbContext _dbContext, IEventPublisher eventProducer)
{
    public async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
    {
        var messages = await _dbContext.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                await eventProducer.ProduceAsync(message, cancellationToken);
                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                message.Error = ex.ToString();
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}