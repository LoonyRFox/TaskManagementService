using MediatR;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Application.Interfaces.Repositories;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Queries.GetPagedListTask;

public class GetPagedListTaskQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetPagedListTaskQuery, PagedResponse<TaskDto>>
{
    public  async Task<PagedResponse<TaskDto>> Handle(GetPagedListTaskQuery request, CancellationToken cancellationToken)
    {
        return await taskRepository.GetPagedListAsync(request.PageNumber, request.PageSize, request.Title);
    }
}