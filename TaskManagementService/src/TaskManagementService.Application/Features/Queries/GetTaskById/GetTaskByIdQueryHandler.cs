using AutoMapper;
using MediatR;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Application.Helpers;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Application.Interfaces.Repositories;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Queries.GetTaskById;

public class GetTaskByIdQueryHandler(ITaskRepository taskRepository, ITranslator translator, IMapper mapper) : IRequestHandler<GetTaskByIdQuery, BaseResult<TaskDto>>
{
    public  async Task<BaseResult<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.Id);

        if (task == null) return new Error(ErrorCode.NotFound, translator.GetString(TranslatorMessages.TaskMessages.Task_NotFound_with_id(request.Id)), nameof(request.Id));

        return mapper.Map<TaskDto>(task);
    }
}