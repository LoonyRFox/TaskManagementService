using Microsoft.Extensions.Logging;
using System.Text.Json;
using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Application.Wrappers;
using TaskManagementServiceLoging.Domain;

namespace TaskManagementServiceLoging.Application.Validator;

public class LogIncomingMessageValidator  : ILogIncomingMessageValidator
{
    private readonly ILogger _logger;

    public LogIncomingMessageValidator(ILogger<LogIncomingMessageValidator> logger)
    {
        _logger = logger;
    }

    public BaseResult<OutboxMessage> Validate(string message)
    {
        OutboxMessage? outboxMessage = null;

        try
        {
            outboxMessage = JsonSerializer.Deserialize<OutboxMessage>(message);

            if (outboxMessage == null)
            {
                return BaseResult<OutboxMessage>.Failure(
                    new Error(ErrorCode.ValidationError, "OUTBOX_MESSAGE_IS_NULL", "Сообщение пустое или некорректное.")
                );
            }

            // Простейшие проверки
            if (outboxMessage.Id == Guid.Empty)
            {
                return BaseResult<OutboxMessage>.Failure(
                    new Error(ErrorCode.ValidationError, "INVALID_ID", "Id сообщения пуст.")
                );
            }

            if (string.IsNullOrWhiteSpace(outboxMessage.Type))
            {
                return BaseResult<OutboxMessage>.Failure(
                    new Error(ErrorCode.ValidationError, "INVALID_TYPE", "Тип сообщения не указан.")
                );
            }

            if (string.IsNullOrWhiteSpace(outboxMessage.Payload))
            {
                return BaseResult<OutboxMessage>.Failure(
                    new Error(ErrorCode.ValidationError, "INVALID_PAYLOAD", "Payload пуст.")
                );
            }

            return BaseResult<OutboxMessage>.Ok(outboxMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка валидации OutboxMessage: {error}", ex.Message);

            var error = new Error(ErrorCode.Exception, "VALIDATION_OUTBOX_MESSAGE_ERROR", ex.Message);
            
            return BaseResult<OutboxMessage>.Failure( error);
        }
    }
}