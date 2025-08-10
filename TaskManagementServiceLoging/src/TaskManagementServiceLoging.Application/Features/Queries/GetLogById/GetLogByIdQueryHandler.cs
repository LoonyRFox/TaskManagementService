using AutoMapper;
using MediatR;
using TaskManagementServiceLoging.Application.DTOs.Log;
using TaskManagementServiceLoging.Application.Interfaces.Repositories;
using TaskManagementServiceLoging.Application.Wrappers;

namespace TaskManagementServiceLoging.Application.Features.Queries.GetLogById;

public class GetLogByIdQueryHandler(ILogRepository logRepository, IMapper mapper) : IRequestHandler<GetLogByIdQuery, BaseResult<LogDto>>
{
    public async Task<BaseResult<LogDto>> Handle(GetLogByIdQuery request, CancellationToken cancellationToken)
    {
        var log = await logRepository.GetByIdAsync(request.Id);
        if(log is null) return new Error(ErrorCode.NotFound, $" Log record not found with id: {request.Id}", nameof(request.Id));

        return mapper.Map<LogDto>(log);
    }
}