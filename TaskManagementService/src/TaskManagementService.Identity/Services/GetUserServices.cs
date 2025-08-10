using Microsoft.EntityFrameworkCore;
using TaskManagementService.Application.DTOs;
using TaskManagementService.Application.DTOs.Account.Requests;
using TaskManagementService.Application.DTOs.Account.Responses;
using TaskManagementService.Application.Interfaces.UserInterfaces;
using TaskManagementService.Application.Wrappers;
using TaskManagementService.Identity.Context;

namespace TaskManagementService.Identity.Services;

public class GetUserServices(IdentityContext identityContext) : IGetUserServices
{
    public async Task<PagedResponse<UserDto>> GetPagedUsers(GetAllUsersRequest model)
    {
        var skip = (model.PageNumber - 1) * model.PageSize;

        var users = identityContext.Users
            .Select(p => new UserDto()
            {
                Name = p.Name,
                Email = p.Email,
                UserName = p.UserName,
                PhoneNumber = p.PhoneNumber,
                Id = p.Id,
                Created = p.Created,
            });

        if (!string.IsNullOrEmpty(model.Name))
        {
            users = users.Where(p => p.Name.Contains(model.Name));
        }

        return new PaginationResponseDto<UserDto>(
            await users.Skip(skip).Take(model.PageSize).ToListAsync(),
            await users.CountAsync(),
            model.PageNumber,
            model.PageSize);

    }
}