using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementService.Application.DTOs.Task;
using TaskManagementService.Application.Features.Commands.CreateTask;
using TaskManagementService.Application.Features.Commands.DeleteTask;
using TaskManagementService.Application.Features.Commands.UpdateTask;
using TaskManagementService.Application.Features.Queries.GetPagedListTask;
using TaskManagementService.Application.Features.Queries.GetTaskById;
using TaskManagementService.Application.Wrappers;
using TaskManagementService.Domain.Entity;

namespace TaskManagementService.WebApi.Controllers.v1;
[ApiVersion("1")]
public class TaskController : BaseApiController
{
    // GET
    [HttpGet]
    public async Task<PagedResponse<TaskDto>> GetPagedListTask([FromQuery] GetPagedListTaskQuery model)
        => await Mediator.Send(model);

    [HttpGet]
    public async Task<BaseResult<TaskDto>> GetTaskById([FromQuery] GetTaskByIdQuery model)
        => await Mediator.Send(model);

    [HttpPost, Authorize]
    public async Task<BaseResult<long>> CreateTask(CreateTaskCommand model)
        => await Mediator.Send(model);

    [HttpPut, Authorize]
    public async Task<BaseResult> UpdateTask(UpdateTaskCommand model)
        => await Mediator.Send(model);

    [HttpDelete, Authorize]
    public async Task<BaseResult> DeleteTask([FromQuery] DeleteTaskCommand model)
        => await Mediator.Send(model);
}