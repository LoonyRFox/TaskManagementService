using Microsoft.EntityFrameworkCore;

using TaskManagementServiceLoging.Infrastructure.Context;

namespace TaskManagementServiceLoging.WebApi.Configurations;

public static class MigrationConfigurations
{
    public static IApplicationBuilder ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

     

        return app;
    }
}