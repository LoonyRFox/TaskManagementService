using MediatR;
using TaskManagementService.Application.Helpers;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.Interfaces.Repositories;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Commands.DeleteTask;

public class DeleteTaskCommandHandler   (ITaskRepository taskRepository, IUnitOfWork unitOfWork, ITranslator translator) : IRequestHandler<DeleteTaskCommand,BaseResult>
{
    public async Task<BaseResult> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.Id);

        if (task is null)
        {
            return new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.TaskMessages.Task_NotFound_with_id(request.Id)), nameof(request.Id));
        }

        taskRepository.Delete(task);
        await unitOfWork.SaveChangesAsync();

        return BaseResult.Ok();
    }
}