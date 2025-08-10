using AutoMapper;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using TaskManagementService.Application.DTOs;
using TaskManagementService.Application.Interfaces.Messaging;
using TaskManagementService.Domain.Messages;
using TaskManagementService.Messaging.Interfaces;

namespace TaskManagementService.Messaging;

public class HttpEventSender : IEventPublisher
{
    private readonly ITaskManagementLogHttpClient _taskManagementLogHttpClient;
    private readonly ILogger<HttpEventSender> _logger;
    private readonly IMapper _mapper;

    public HttpEventSender(ITaskManagementLogHttpClient taskManagementLogHttpClient, ILogger<HttpEventSender> logger, IMapper mapper)
    {
        _taskManagementLogHttpClient = taskManagementLogHttpClient;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task ProduceAsync(OutboxMessage messageDto, CancellationToken cancellationToken)
    {
        var request = _mapper.Map<CreateLogRequest>(messageDto);
        try
        {
            var response = await _taskManagementLogHttpClient.CreateAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                string messageError = $"Error sending request. Service: {nameof(HttpEventSender)}. StatusCode: {response.StatusCode}. Error: {response.Error}";
                _logger.LogError(messageError);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"[{nameof(HttpEventSender)}]: HttpSender error: {ex.Message}");
        }
    }
}