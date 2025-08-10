using MediatR;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest<BaseResult>
{
     public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
}