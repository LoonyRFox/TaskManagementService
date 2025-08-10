using MediatR;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest<BaseResult>
{
    public long Id { get; set; }
}