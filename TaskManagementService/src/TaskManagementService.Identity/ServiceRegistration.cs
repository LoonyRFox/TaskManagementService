using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TaskManagementService.Application.Interfaces.UserInterfaces;
using TaskManagementService.Application.Wrappers;
using TaskManagementService.Domain.Configurations;
using TaskManagementService.Identity.Context;
using TaskManagementService.Identity.Models;
using TaskManagementService.Identity.Services;
using TaskManagementService.Identity.Settings;

namespace TaskManagementService.Identity
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services,
            IConfiguration configuration, bool useInMemoryDatabase)
        {
            if (useInMemoryDatabase)
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseInMemoryDatabase(nameof(IdentityContext)));
            }
            else
            {
                var identityDbOptions = new DatabaseOptions();
                configuration.GetSection("IdentityDatabaseOptions").Bind(identityDbOptions);

                var identityConnectionString = $"Host={identityDbOptions.Host};Port={identityDbOptions.Port};Database={identityDbOptions.Name};Username={identityDbOptions.User};Password={identityDbOptions.Password}";

                services.AddDbContext<IdentityContext>(options =>
                    options.UseNpgsql(identityConnectionString));
            }
            services.AddTransient<IGetUserServices, GetUserServices>();
            services.AddTransient<IAccountServices, AccountServices>();
            var identitySettings = configuration.GetSection(nameof(IdentitySettings)).Get<IdentitySettings>();

            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
            services.AddSingleton(jwtSettings);

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = false;

                options.Password.RequireDigit = identitySettings.PasswordRequireDigit;
                options.Password.RequiredLength = identitySettings.PasswordRequiredLength;
                options.Password.RequireNonAlphanumeric = identitySettings.PasswordRequireNonAlphanumeric;
                options.Password.RequireUppercase = identitySettings.PasswordRequireUppercase;
                options.Password.RequireLowercase = identitySettings.PasswordRequireLowercase;

            }).AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnChallenge = async context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsJsonAsync(BaseResult.Failure(new Error(ErrorCode.AccessDenied, "You are not Authorized")));
                        },
                        OnForbidden = async context =>
                        {
                            context.Response.StatusCode = 403;
                            await context.Response.WriteAsJsonAsync(BaseResult.Failure(new Error(ErrorCode.AccessDenied, "You are not authorized to access this resource")));
                        },
                        OnTokenValidated = async context =>
                        {
                            var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
                            if (claimsIdentity?.Claims.Any() is not true)
                                context.Fail("This token has no claims.");

                            var securityStamp = claimsIdentity?.FindFirst("AspNet.Identity.SecurityStamp");
                            if (securityStamp is null)
                                context.Fail("This token has no security stamp");

                            var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<ApplicationUser>>();
                            var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                            if (validatedUser is null)
                                context.Fail("Token security stamp is not valid.");
                        },

                    };
                });

            return services;
        }
    }
}
