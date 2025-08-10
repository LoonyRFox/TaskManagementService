using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using TaskManagementService.Application.Interfaces.Messaging;
using TaskManagementService.Domain.Messages;
using TaskManagementService.Messaging.Configurations;
using TaskManagementService.Messaging.Converters;

namespace TaskManagementService.Messaging.KafkaMessaging;

public class NotificationServiceProducer  :IEventPublisher
{
    private readonly IMessageProducer _messageProducer;
    private readonly string _topic;
    private JsonSerializerOptions _serializeOptions;
    private readonly ILogger _logger;

    public NotificationServiceProducer(IMessageProducer messageProducer,
        IOptions<OutboxOptions> options,
        ILogger<NotificationServiceProducer> logger)
    {
        _messageProducer = messageProducer;
        _topic = options.Value.Kafka.NotificationProducerOptions.Topic;
        _serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        _serializeOptions.Converters.Add(new DateTimeJsonConverter());
        _logger = logger;
    }

    public  async Task ProduceAsync(OutboxMessage? messageDto, CancellationToken cancellationToken)
    {
        try
        {
            string messageText = JsonSerializer.Serialize(messageDto, _serializeOptions);
            var message = new Confluent.Kafka.Message<string, string>()
            {
                Key = null,
                Value = messageText
            };
            await _messageProducer.Produce(_topic, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось отправить сообщение: {error}", ex.Message);
        }
    }
}