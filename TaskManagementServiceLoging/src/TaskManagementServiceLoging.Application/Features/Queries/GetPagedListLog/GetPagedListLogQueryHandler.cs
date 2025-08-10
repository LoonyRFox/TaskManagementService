using MediatR;
using TaskManagementServiceLoging.Application.DTOs.Log;
using TaskManagementServiceLoging.Application.Interfaces.Repositories;
using TaskManagementServiceLoging.Application.Wrappers;

namespace TaskManagementServiceLoging.Application.Features.Queries.GetPagedListLog;

public class GetPagedListLogQueryHandler(ILogRepository logRepository) : IRequestHandler<GetPagedListLogQuery, PagedResponse<LogDto>>
{
    public async Task<PagedResponse<LogDto>> Handle(GetPagedListLogQuery request, CancellationToken cancellationToken)
    {
        return await logRepository.GetPagedListAsync(request.PageNumber, request.PageSize, request.Type);
    }
}