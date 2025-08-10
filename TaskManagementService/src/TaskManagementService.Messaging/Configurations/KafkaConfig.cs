using Confluent.Kafka;
using TaskManagementService.Application.Interfaces.Messaging;

namespace TaskManagementService.Messaging.Configurations;

public class KafkaConfig : ClientConfig   , IKafkaConfig
{
    public NotificationProducerOptions  NotificationProducerOptions { get; set; }
    public SaslMechanism SaslMechanism { get; set; }
    public SecurityProtocol SecurityProtocol { get; set; }
}