using TaskManagementService.Application.DTOs.Account.Requests;
using TaskManagementService.Application.DTOs.Account.Responses;
using TaskManagementService.Application.DTOs.Account.Requests;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Interfaces.UserInterfaces
{
    public interface IAccountServices
    {
        Task<BaseResult<string>> RegisterGhostAccount();
        Task<BaseResult> ChangePassword(ChangePasswordRequest model);
        Task<BaseResult> ChangeUserName(ChangeUserNameRequest model);
        Task<BaseResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request);
        Task<BaseResult<AuthenticationResponse>> AuthenticateByUserName(string username);

    }
}
