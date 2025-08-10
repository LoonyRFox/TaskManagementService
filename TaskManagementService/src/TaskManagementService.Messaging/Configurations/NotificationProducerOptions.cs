using Confluent.Kafka;
using TaskManagementService.Application.Interfaces.Messaging;

namespace TaskManagementService.Messaging.Configurations;

public class NotificationProducerOptions : ProducerConfig, IProducerConfig
{
    public int RetryOnFailedDelayMs { get; set; }
    public string Topic { get; set; }
}