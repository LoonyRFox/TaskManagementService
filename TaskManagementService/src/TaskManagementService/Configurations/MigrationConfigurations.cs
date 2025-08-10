using Microsoft.EntityFrameworkCore;
using TaskManagementService.Identity.Context;
using TaskManagementService.Persistence.Context;

namespace TaskManagementService.WebApi.Configurations;

public static class MigrationConfigurations
{
    public static IApplicationBuilder ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext1 = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext1.Database.Migrate();

        var dbContext2 = scope.ServiceProvider.GetRequiredService<IdentityContext>();
        dbContext2.Database.Migrate();

        return app;
    }
}