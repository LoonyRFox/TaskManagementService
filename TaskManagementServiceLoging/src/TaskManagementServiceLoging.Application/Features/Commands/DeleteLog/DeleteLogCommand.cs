using MediatR;
using TaskManagementServiceLoging.Application.Wrappers;

namespace TaskManagementServiceLoging.Application.Features.Commands.DeleteLog;

public class DeleteLogCommand : IRequest<BaseResult>
{
    public long Id { get; set; }
}