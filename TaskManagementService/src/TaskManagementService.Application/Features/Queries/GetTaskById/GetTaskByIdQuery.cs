using MediatR;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Queries.GetTaskById;

public class GetTaskByIdQuery : IRequest<BaseResult<TaskDto>>
{
    public long Id { get; set; }
}