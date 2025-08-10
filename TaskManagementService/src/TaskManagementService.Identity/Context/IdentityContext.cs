using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagementService.Identity.Models;

namespace TaskManagementService.Identity.Context;
public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private readonly string _schema;

    public IdentityContext(DbContextOptions<IdentityContext> options, IConfiguration configuration)
        : base(options)
    {
        // Читаем схему из конфига (по умолчанию "identity")
        _schema = configuration.GetValue<string>("IdentityDatabaseOptions:Schema") ?? "identity";
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(_schema);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("User");
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("Role");
        });

        builder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("UserRoles");
        });

        builder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("UserClaims");
        });

        builder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("UserLogins");
        });

        builder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("RoleClaims");
        });

        builder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("UserTokens");
        });
    }
}