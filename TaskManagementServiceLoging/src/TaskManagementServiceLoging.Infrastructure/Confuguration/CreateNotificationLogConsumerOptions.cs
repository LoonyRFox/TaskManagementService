using Confluent.Kafka;
using TaskManagementServiceLoging.Application.Interfaces;

namespace TaskManagementServiceLoging.Infrastructure.Confuguration;

public class CreateNotificationLogConsumerOptions : ConsumerConfig, IConsumerConfig
{
    public string Topic { get; set; }
    public int RetryOnEmptyDelayMs { get; set; }
    public int RetryOnFailedDelayMs { get; set; }
    public int ConsumeTimeoutMs { get; set; }
}