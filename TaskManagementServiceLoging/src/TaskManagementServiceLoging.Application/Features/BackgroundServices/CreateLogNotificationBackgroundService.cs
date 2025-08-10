using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System.Net;
using System.Threading;
using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Application.Interfaces.Repositories;
using TaskManagementServiceLoging.Domain;
using TaskManagementServiceLoging.Domain.Entity;
using TaskManagementServiceLoging.Domain.Uptrace;

namespace TaskManagementServiceLoging.Application.Features.BackgroundServices;

public class CreateLogNotificationBackgroundService : BackgroundServiceBase<OutboxMessage>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IServiceNotificationlogConsumer _serviceNotificationlogConsumer;
    private readonly ILogIncomingMessageValidator _incomingMessageValidator;
    
    private readonly IMapper _mapper;
    

    public CreateLogNotificationBackgroundService(IServiceNotificationlogConsumer serviceNotificationlogConsumer, ILogIncomingMessageValidator incomingMessageValidator,
        IMapper mapper, 
        OpenTelemetryHolder openTelemetryHolder,
        IServiceScopeFactory scopeFactory) : base(nameof(CreateLogNotificationBackgroundService), openTelemetryHolder)
    {
        _serviceNotificationlogConsumer = serviceNotificationlogConsumer;
        _incomingMessageValidator = incomingMessageValidator;
        
        _mapper = mapper;
        
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        await ListenTopic(stoppingToken);
    }
    private async Task ListenTopic(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            var incomingMessage = await _serviceNotificationlogConsumer.ConsumeAsync();
            var validateResult = _incomingMessageValidator.Validate(incomingMessage);
            await ProcessMessage(validateResult.Data);

            await _serviceNotificationlogConsumer.CommitDataAsync();
        }
    }


    protected override async Task WorkWithMessageAsync(OutboxMessage? message)
    {
        if (message == null) return;

        using var scope = _scopeFactory.CreateScope();

        var logRepository = scope.ServiceProvider.GetRequiredService<ILogRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var logItem = new LogModel(
            message.Id,
            message.Type,
            message.Payload,
            message.ProcessedOnUtc,
            message.Error
        );

        await logRepository.AddAsync(logItem);
        await unitOfWork.SaveChangesAsync();
    }
}