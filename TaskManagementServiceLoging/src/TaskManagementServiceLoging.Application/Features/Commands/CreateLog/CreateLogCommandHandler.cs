using MediatR;

using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Application.Interfaces.Repositories;
using TaskManagementServiceLoging.Application.Wrappers;
using TaskManagementServiceLoging.Domain.Entity;

namespace TaskManagementServiceLoging.Application.Features.Commands.CreateLog;

public class CreateTaskCommandHandler (ILogRepository logRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateLogCommand, BaseResult<long>>
{
    public async Task<BaseResult<long>> Handle(CreateLogCommand request, CancellationToken cancellationToken)
    {
        var log = new LogModel(request.LogId, request.Type, request.Payload, request.ProcessedOnUtc, request.Error);

        await logRepository.AddAsync(log);
        await unitOfWork.SaveChangesAsync();

        return log.Id;
    }
}