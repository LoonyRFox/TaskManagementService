using MediatR;
using TaskManagementServiceLoging.Application.DTOs.Log;
using TaskManagementServiceLoging.Application.Wrappers;

namespace TaskManagementServiceLoging.Application.Features.Queries.GetLogById;

public class GetLogByIdQuery : IRequest<BaseResult<LogDto>>
{
    public long Id { get; set; }
}