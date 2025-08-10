using Confluent.Kafka;
using Microsoft.Extensions.Options;
using TaskManagementService.Application.Interfaces.Messaging;
using TaskManagementService.Messaging.Configurations;

namespace TaskManagementService.Messaging.KafkaMessaging;

public class KafkaProducer : IMessageProducer, IDisposable
{
    private readonly Dictionary<string, IProducer<string, string>> _producers = new Dictionary<string, IProducer<string, string>>();

    public KafkaProducer(IOptions<OutboxOptions> config)
    {
        if (config == null)
            throw new ArgumentNullException("config");

        SetKafkaValues(config.Value.Kafka.NotificationProducerOptions, config.Value.Kafka);

        if (config.Value.Kafka.NotificationProducerOptions.Topic == null)
            throw new ArgumentNullException("NotificationProducerOptions.Topic");

        _producers.Add(config.Value.Kafka.NotificationProducerOptions.Topic,
            new ProducerBuilder<string, string>(config.Value.Kafka.NotificationProducerOptions)
                .Build());
    }

    public async Task Produce(string topic, Message<string, string> message)
    {
        if (!_producers.TryGetValue(topic, out var producer))
            throw new ArgumentException($"Producer for topic {topic} not found.");

        var res = await producer.ProduceAsync(topic, message);
    }

    public void Dispose()
    {
        foreach (var producer in _producers)
        {
            producer.Value.Dispose();
        }
    }

    public static void SetKafkaValues(ProducerConfig recipientConfig, IKafkaConfig config)
    {
        recipientConfig.BootstrapServers = config.BootstrapServers;
        //recipientConfig.SecurityProtocol = config.SecurityProtocol;
        //recipientConfig.SaslMechanism = config.SaslMechanism;
        //recipientConfig.SaslUsername = config.SaslUsername;
        //recipientConfig.SaslPassword = config.SaslPassword;
    }
}