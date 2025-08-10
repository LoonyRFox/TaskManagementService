using TaskManagementService.Application.DTOs.Account.Requests;
using TaskManagementService.Application.DTOs.Account.Responses;
using TaskManagementService.Application.Wrappers;

namespace TaskManagementService.Application.Interfaces.UserInterfaces
{
    public interface IGetUserServices
    {
        Task<PagedResponse<UserDto>> GetPagedUsers(GetAllUsersRequest model);
    }
}
