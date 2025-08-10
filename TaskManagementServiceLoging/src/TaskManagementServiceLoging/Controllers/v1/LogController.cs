using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using TaskManagementServiceLoging.Application.DTOs.Log;
using TaskManagementServiceLoging.Application.Features.Commands.CreateLog;
using TaskManagementServiceLoging.Application.Features.Commands.DeleteLog;
using TaskManagementServiceLoging.Application.Features.Queries.GetLogById;
using TaskManagementServiceLoging.Application.Features.Queries.GetPagedListLog;
using TaskManagementServiceLoging.Application.Wrappers;
using TaskManagementServiceLoging.WebApi.Controllers;

namespace TaskManagementServiceLoging.WebApi.Controllers.v1;

[ApiVersion("1")]
public class LogController : BaseApiController
{
    // GET
    [HttpGet]
    public async Task<PagedResponse<LogDto>> GetPagedListTask([FromQuery] GetPagedListLogQuery model)
        => await Mediator.Send(model);

    [HttpGet]
    public async Task<BaseResult<LogDto>> GetTaskById([FromQuery] GetLogByIdQuery model)
        => await Mediator.Send(model);

    [HttpPost]
    public async Task<BaseResult<long>> CreateTask(CreateLogCommand model)
        => await Mediator.Send(model);


    [HttpDelete]
    public async Task<BaseResult> DeleteTask([FromQuery] DeleteLogCommand model)
        => await Mediator.Send(model);
}