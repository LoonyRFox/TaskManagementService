using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Application.Interfaces.MessageConsumerFactory;
using TaskManagementServiceLoging.Infrastructure.Confuguration;

namespace TaskManagementServiceLoging.Infrastructure.Messaging;

public class ServiceNotificationlogConsumer  : IServiceNotificationlogConsumer
{
    private readonly IMessageConsumer _messageConsumer;
    private readonly ILogger _logger;

    public ServiceNotificationlogConsumer(IMessageConsumerFactory messageConsumerFactory,
        IOptions<KafkaConfig> options,
        ILogger<ServiceNotificationlogConsumer> logger)
    {
        _messageConsumer = messageConsumerFactory.CreateConsumer(options.Value.CreateNotificationLogConsumerOptions.Topic);
        _logger = logger;
    }

    public async Task CommitDataAsync()
    {
        await _messageConsumer.CommitDataAsync();
    }

    public async Task<string> ConsumeAsync()
    {
        try
        {
            return await _messageConsumer.ConsumeRawAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка получения сообщения: {error}", ex.Message);
            return null;
        }
    }
}