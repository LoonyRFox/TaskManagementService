using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementService.Application.DTOs.Account.Requests;
using TaskManagementService.Application.DTOs.Account.Responses;
using TaskManagementService.Application.Interfaces.UserInterfaces;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.WebApi.Controllers.v1;

[ApiVersion("1")]
public class AccountController(IAccountServices accountServices) : BaseApiController
{
    [HttpPost]
    public async Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
        => await accountServices.Authenticate(request);

    [HttpPut, Authorize]
    public async Task<BaseResult> ChangeUserName(ChangeUserNameRequest model)
        => await accountServices.ChangeUserName(model);

    [HttpPut, Authorize]
    public async Task<BaseResult> ChangePassword(ChangePasswordRequest model)
        => await accountServices.ChangePassword(model);

    [HttpPost]
    public async Task<BaseResult<AuthenticationResponse>> Start()
    {
        var ghostUsername = await accountServices.RegisterGhostAccount();
        return await accountServices.AuthenticateByUserName(ghostUsername.Data);
    }
}