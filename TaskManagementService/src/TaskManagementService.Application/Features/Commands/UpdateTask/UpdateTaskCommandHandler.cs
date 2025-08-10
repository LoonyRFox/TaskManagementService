using MediatR;
using TaskManagementService.Application.Helpers;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.Interfaces.Repositories;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Commands.UpdateTask;

public class UpdateTaskCommandHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork, ITranslator translator)  : IRequestHandler<UpdateTaskCommand, BaseResult>
{
    public async Task<BaseResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.Id);

        if (task == null)
            return new Error(ErrorCode.NotFound,
                translator.GetString(TranslatorMessages.TaskMessages.Task_NotFound_with_id(request.Id)),
                nameof(request.Id));

        task.Update(request.Title,request.Description, request.Status);
        await unitOfWork.SaveChangesAsync();
        return BaseResult.Ok();
    }
}