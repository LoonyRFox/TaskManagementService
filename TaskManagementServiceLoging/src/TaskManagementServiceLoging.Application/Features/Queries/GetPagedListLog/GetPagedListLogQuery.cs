using MediatR;
using TaskManagementServiceLoging.Application.DTOs.Log;
using TaskManagementServiceLoging.Application.Parameters;
using TaskManagementServiceLoging.Application.Wrappers;

namespace TaskManagementServiceLoging.Application.Features.Queries.GetPagedListLog;

public class GetPagedListLogQuery : PaginationRequestParameter, IRequest<PagedResponse<LogDto>>
{
    public string? Type { get; set; }
}