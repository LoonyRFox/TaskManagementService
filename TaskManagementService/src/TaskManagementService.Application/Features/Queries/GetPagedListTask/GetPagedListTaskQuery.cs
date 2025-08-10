using MediatR;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Application.Parameters;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Queries.GetPagedListTask;

public class GetPagedListTaskQuery : PaginationRequestParameter, IRequest<PagedResponse<TaskDto>>
{
    public string? Title { get; set; }
}