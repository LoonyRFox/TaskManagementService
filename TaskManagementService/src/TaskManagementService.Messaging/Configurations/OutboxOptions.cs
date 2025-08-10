namespace TaskManagementService.Messaging.Configurations;

public class OutboxOptions
{
    public string DeliveryMode { get; set; } = "Kafka";
    public KafkaConfig Kafka { get; set; } = new();
    public HttpConfig Http { get; set; } = new();
}