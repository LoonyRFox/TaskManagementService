using MediatR;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.Interfaces.Repositories;
using TaskManagementService.Application.Wrappers;
using TaskManagementService.Domain.Entity;

namespace TaskManagementService.Application.Features.Commands.CreateTask;

public class CreateTaskCommandHandler (ITaskRepository taskRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateTaskCommand, BaseResult<long>>
{
    public async Task<BaseResult<long>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskItem(request.Title,request.Description, nameof(TaskManagementService.Domain.Enums.CustomTaskStatus.New));

        await taskRepository.AddAsync(task);
        await unitOfWork.SaveChangesAsync();

        return task.Id;
    }
}