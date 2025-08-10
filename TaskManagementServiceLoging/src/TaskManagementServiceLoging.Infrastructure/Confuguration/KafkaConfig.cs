using Confluent.Kafka;
using TaskManagementServiceLoging.Application.Interfaces;

namespace TaskManagementServiceLoging.Infrastructure.Confuguration;

public class KafkaConfig : ClientConfig, IKafkaConfig
{
    public const string SECTION_NAME = "KafkaConfig";
    public SaslMechanism SaslMechanism { get; set; }
    public SecurityProtocol SecurityProtocol { get; set; }
    public CreateNotificationLogConsumerOptions CreateNotificationLogConsumerOptions { get; set; }
}