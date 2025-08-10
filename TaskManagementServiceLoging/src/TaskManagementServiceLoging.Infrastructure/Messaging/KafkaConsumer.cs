using Confluent.Kafka;
using System.Text.Json;
using Microsoft.Extensions.Logging;

using Microsoft.Extensions.Options;
using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Infrastructure.Confuguration;

namespace TaskManagementServiceLoging.Infrastructure.Messaging;

public class KafkaConsumer : IMessageConsumer
{
    private readonly IConsumerConfig _config;
    private IConsumer<Ignore, string> _consumer { get; set; }
    private readonly ILogger<KafkaConsumer> _logger;
    private ConsumeResult<Ignore, string>? consumedMessage;
    private string _topic;

    public KafkaConsumer(string topic, IOptions<KafkaConfig> config, ILogger<KafkaConsumer> logger)
    {
        _logger = logger;
        if (config == null)
        {
            _logger.LogCritical("Отсутствует конфигурация кафки");
            throw new ArgumentNullException("Kafka config");
        }
        var kafkaConfig = config;

        if (config.Value.CreateNotificationLogConsumerOptions.Topic == topic)
        {
            _topic = config.Value.CreateNotificationLogConsumerOptions.Topic;
            _config = config.Value.CreateNotificationLogConsumerOptions;
            SetKafkaValues(config.Value.CreateNotificationLogConsumerOptions, config.Value);
            _consumer = new ConsumerBuilder<Ignore, string>(config.Value.CreateNotificationLogConsumerOptions).Build();
        }

        if (_consumer is null)
        {
            _logger.LogCritical("Отсутствует конфигурация consumer");
            throw new ArgumentNullException("Consumer config");
        }
        _consumer.Subscribe(_topic);
    }

    public async Task<T> ConsumeAsync<T>()
    {
        consumedMessage = null;
        while (true)
        {
            _logger.LogInformation("[{topic}] Получение нового сообщения", _topic);

            try
            {
                consumedMessage = _consumer.Consume(_config.ConsumeTimeoutMs);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("[{topic}] Не удалось получить новое сообщение: {err}. Следующая попытка через {delay} секунд",
                    _topic, ex.Message, _config.RetryOnFailedDelayMs / 1000);
                await Task.Delay(_config.RetryOnFailedDelayMs);
                continue;
            }

            if (consumedMessage == null)
            {
                _logger.LogInformation("[{topic}] Нет новых сообщений", _topic);
                await Task.Delay(_config.RetryOnEmptyDelayMs);
            }
            else
            {
                _logger.LogInformation("[{topic}] Получено сообщение (Offset = {offset}, TopicPartitionOffset = {topicPartitionOffset}):{newLine}{message}",
                    _topic, consumedMessage.Offset, consumedMessage.TopicPartitionOffset, Environment.NewLine, consumedMessage.Message.Value);
                return JsonSerializer.Deserialize<T>(consumedMessage.Message.Value);
            }
        }
    }

    public async Task<string> ConsumeRawAsync()
    {
        consumedMessage = null;
        while (true)
        {
            _logger.LogInformation("[{topic}] Получение нового сообщения", _topic);

            try
            {
                consumedMessage = _consumer.Consume(_config.ConsumeTimeoutMs);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("[{topic}] Не удалось получить новое сообщение: {err}. Следующая попытка через {delay} секунд",
                    _topic, ex.Message, _config.RetryOnFailedDelayMs / 1000);
                await Task.Delay(_config.RetryOnFailedDelayMs);
                continue;
            }

            if (consumedMessage == null)
            {
                _logger.LogInformation("[{topic}] Нет новых сообщений", _topic);
                await Task.Delay(_config.RetryOnEmptyDelayMs);
            }
            else
            {
                _logger.LogInformation("[{topic}] Получено сообщение (Offset = {offset}, TopicPartitionOffset = {topicPartitionOffset}):{newLine}{message}",
                    _topic, consumedMessage.Offset, consumedMessage.TopicPartitionOffset, Environment.NewLine, consumedMessage.Message.Value);
                return consumedMessage.Message.Value;
            }
        }
    }

    public void Dispose()
    {
        _consumer.Dispose();
    }

    public async Task CommitDataAsync()
    {
        if (consumedMessage == null)
        {
            return;
        }

        while (true)
        {
            try
            {
                _logger.LogInformation("[{topic}] Выполняется коммит", _topic);
                _consumer.Commit(consumedMessage);
                _logger.LogInformation("[{topic}] Коммит выполнен успешно", _topic);
                consumedMessage = null;
                break;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("[{topic}] Не удалось выполнить коммит: {err}. Следующая попытка через {delay} секунд",
                    _topic, ex.Message, _config.RetryOnFailedDelayMs / 1000);
                await Task.Delay(_config.RetryOnFailedDelayMs);
            }
        }
    }

    public static void SetKafkaValues(ConsumerConfig consumerConfig, IKafkaConfig config)
    {
        consumerConfig.BootstrapServers = config.BootstrapServers;
        consumerConfig.SecurityProtocol = config.SecurityProtocol;
        consumerConfig.SaslMechanism = config.SaslMechanism;
        consumerConfig.SaslUsername = config.SaslUsername;
        consumerConfig.SaslPassword = config.SaslPassword;
    }
}