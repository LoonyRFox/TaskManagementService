using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagementService.Application;
using TaskManagementService.Application.Interfaces;
using TaskManagementService.Identity;
using TaskManagementService.Identity.Context;
using TaskManagementService.Identity.Models;
using TaskManagementService.Identity.Seed;
using TaskManagementService.Messaging;
using TaskManagementService.Persistence;
using TaskManagementService.Persistence.Context;
using TaskManagementService.Persistence.Seed;
using TaskManagementService.Resources;
using TaskManagementService.WebApi.Configurations;
using TaskManagementService.WebApi.Infrastructure.Extensions;
using TaskManagementService.WebApi.Infrastructure.Middlewares;
using TaskManagementService.WebApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);
bool useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");

// Add services to the container.

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration, useInMemoryDatabase);
builder.Services.AddIdentityInfrastructure(builder.Configuration, useInMemoryDatabase);
builder.Services.AddResourcesInfrastructure();
builder.Services.AddMessagingInfrastructure();
builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddControllers();
builder.Services.AddVersioning();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSwaggerWithVersioning();
builder.Services.AddAnyCors();
builder.Services.AddCustomLocalization(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    if (!useInMemoryDatabase)
    {
        await services.GetRequiredService<IdentityContext>().Database.MigrateAsync();
        await services.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    }

    //Seed Data
    await DefaultRoles.SeedAsync(services.GetRequiredService<RoleManager<ApplicationRole>>());
    await DefaultBasicUser.SeedAsync(services.GetRequiredService<UserManager<ApplicationUser>>());
    await TaskDefaultData.SeedAsync(services.GetRequiredService<ApplicationDbContext>());
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var url = "http://localhost:5250/swagger";

    // Запускаем браузер с Swagger UI
    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }
    catch
    {
        // Игнорируем ошибки, если не удалось запустить браузер
    }
}

app.UseCustomLocalization();
app.UseAnyCors();
app.UseRouting();
app.ApplyMigrations();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerWithVersioning();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHealthChecks("/health");
app.MapControllers();
app.UseSerilogRequestLogging();

app.Run();
public partial class Program
{
}