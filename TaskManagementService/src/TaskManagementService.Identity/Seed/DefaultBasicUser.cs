using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementService.Identity.Models;

namespace TaskManagementService.Identity.Seed;

public static class DefaultBasicUser
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        //Seed Default User
        var defaultUser = new ApplicationUser
        {
            UserName = "Admin",
            Email = "Admin@Admin.com",
            Name = "Saman",
            PhoneNumber = "09304241296",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if (!await userManager.Users.AnyAsync())
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Sam@12345");
            }
        }
    }
}