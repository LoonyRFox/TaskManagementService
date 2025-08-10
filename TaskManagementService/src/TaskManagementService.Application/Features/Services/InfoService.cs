using Confluent.Kafka;
using System.Reflection;
using TaskManagementService.Application.DTOs.Info;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.Interfaces.Messaging;

namespace TaskManagementService.Application.Features.Services;

public class InfoService : IInfoService
{
    private readonly IKafkaConfig _kafkaConfig;

    public InfoService(IKafkaConfig kafkaConfig)
    {
        _kafkaConfig = kafkaConfig;
    }

    public async Task<HealthCheckResponse> HealthCheckAsync()
    {
        try
        {
            CheckKafka();

            return new HealthCheckResponse() { Status = "OK", Message = $"Service configured" };
        }
        catch (Exception e)
        {
            return new HealthCheckResponse() { Status = "Failed", Message = $"{e.Message}" };
        }
    }

    private void CheckKafka()
    {
        var topics = ValidateKafkaTopics();
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaConfig.BootstrapServers,
            SocketTimeoutMs = 5000
        };

        using var adminClient = new AdminClientBuilder(config).Build();
        foreach (var topic in topics)
        {
            var metadata = adminClient.GetMetadata(topic, TimeSpan.FromSeconds(5));

            if (!metadata.Topics.Exists(t => t.Topic == topic && t.Error.IsError == false))
                throw new Exception($"Topic '{topic}' is not available.");
        }
    }

    private List<string> ValidateKafkaTopics()
    {
        var topics = new List<string>();
        var properties = _kafkaConfig.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.DeclaringType == _kafkaConfig.GetType());
        foreach (var property in properties)
        {
            var propertyValue = property.GetValue(_kafkaConfig);
            if (propertyValue != null)
            {
                var topicProperty = propertyValue.GetType()
                    .GetProperty("Topic", BindingFlags.Public | BindingFlags.Instance);

                if (topicProperty != null)
                {
                    var topicValue = topicProperty.GetValue(propertyValue) as string;
                    if (string.IsNullOrWhiteSpace(topicValue))
                        throw new Exception($"Свойство {property.Name} имеет пустое поле Topic.");

                    topics.Add(topicValue);
                }
            }
        }

        return topics;
    }
}