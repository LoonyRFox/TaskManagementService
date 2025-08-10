using Microsoft.AspNetCore.Identity;

namespace TaskManagementService.Identity.Models;

public class ApplicationRole(string name) : IdentityRole<Guid>(name)
{
}