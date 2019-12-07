using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using Timesheet.Business.Contracts;
using Timesheet.Business.Services;
using Timesheets.Domain.Models;
using Timesheets.Persistence;
using Timesheets.Persistence.Repositories;

namespace Timesheets.Web.Extensions
{
    /// <summary>
    /// Start Up Extension Methods
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Injects Domain repositories to DI Services
        /// </summary>
        /// <param name="services"></param>
        public static void InjectRepositories(this IServiceCollection services)
        {
            services.AddScoped<GenericRepository<Role, int>>();
            services.AddScoped<GenericRepository<User, int>>();
        }

        /// <summary>
        /// Injects Domain Business Objects to DI Services
        /// </summary>
        /// <param name="services"></param>
        public static void InjectServices(this IServiceCollection services)
        {
            services.AddScoped<IRoleService, RolesService>();
            services.AddScoped<IUserService, UserService>();
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<TimesheetDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 2;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
        }
    }
}
