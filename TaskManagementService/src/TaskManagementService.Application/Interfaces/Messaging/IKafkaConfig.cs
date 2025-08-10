using Confluent.Kafka;

namespace TaskManagementService.Application.Interfaces.Messaging;

public interface IKafkaConfig
{
    public string BootstrapServers { get; set; }
    public string SaslUsername { get; set; }
    public string SaslPassword { get; set; }
    public SaslMechanism SaslMechanism { get; set; }
    public SecurityProtocol SecurityProtocol { get; set; }
}