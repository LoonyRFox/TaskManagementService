using MediatR;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Features.Commands.CreateTask;

public class CreateTaskCommand : IRequest<BaseResult<long>>
{
    public string Title { get; set; }
    public string Description { get; set; }
}