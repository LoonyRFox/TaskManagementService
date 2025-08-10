using Confluent.Kafka;

namespace TaskManagementService.Application.Interfaces.Messaging;

public interface IMessageProducer
{
    Task Produce(string topic, Message<string, string> message);
}