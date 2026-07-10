using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LOTONetMonitor.Application.Services;
using LOTONetMonitor.Application.Services.Interfaces;
using LOTONetMonitor.Domain.Entities;
using LOTONetMonitor.Domain.Interfaces;
using LOTONetMonitor.Persistence.Data;
using LOTONetMonitor.Persistence.UnitOfWork;

namespace LOTONetMonitor.Web.Extensions
{
    /// <summary>
    /// Extension method for registering services in dependency injection container
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add database and EF Core services
        /// </summary>
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelaySeconds: 30, errorNumbersToAdd: null);
                    })
            );

            return services;
        }

        /// <summary>
        /// Add ASP.NET Identity services
        /// </summary>
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        /// <summary>
        /// Add application services
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repository and Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Authentication and User services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
