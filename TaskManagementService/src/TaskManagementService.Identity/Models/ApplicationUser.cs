using Microsoft.AspNetCore.Identity;

namespace TaskManagementService.Identity.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}