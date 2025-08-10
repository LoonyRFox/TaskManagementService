using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Application.Interfaces.MessageConsumerFactory;
using TaskManagementServiceLoging.Infrastructure.Confuguration;

namespace TaskManagementServiceLoging.Infrastructure.Messaging;

public class KafkaConsumerFactory : IMessageConsumerFactory
{
    private readonly IOptions<KafkaConfig> _options;
    private readonly ILogger<KafkaConsumer> _logger;

    public KafkaConsumerFactory(IOptions<KafkaConfig> config, ILogger<KafkaConsumer> logger)
    {
        _logger = logger;
        _options = config;
    }

    public IMessageConsumer CreateConsumer(string topic)
    {
        return new KafkaConsumer(topic, _options, _logger);
    }
}