using MediatR;
using TaskManagementServiceLoging.Application.Features.Commands.CreateLog;
using TaskManagementServiceLoging.Application.Interfaces;
using TaskManagementServiceLoging.Application.Interfaces.Repositories;
using TaskManagementServiceLoging.Application.Wrappers;

namespace TaskManagementServiceLoging.Application.Features.Commands.DeleteLog;

public class DeleteLogCommandHandler(ILogRepository logRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteLogCommand, BaseResult>
{
    public async Task<BaseResult> Handle(DeleteLogCommand request, CancellationToken cancellationToken)
    {
        var task = await logRepository.GetByIdAsync(request.Id);

        if (task is null)
        {
            return new Error(ErrorCode.NotFound, $"Item Not Found with Id : {request.Id}", nameof(request.Id));
        }

        logRepository.Delete(task);
        await unitOfWork.SaveChangesAsync();

        return BaseResult.Ok();
    }
}