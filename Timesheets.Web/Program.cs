using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Timesheet.Business.Contracts;
using Timesheets.Web.Data;

namespace Timesheets.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();

            using (IServiceScope serviceScope = host.Services.CreateScope())
            {
                IServiceProvider services = serviceScope.ServiceProvider;

                try
                {
                    IRoleService roleService = services.GetRequiredService<IRoleService>();
                    IUserService userService = services.GetRequiredService<IUserService>();
                    roleService.SeedRolesAsync(Seed.GetInitialRoles()).Wait();
                    userService.CreateAsync(Seed.GetDefaultAdmin()).Wait();
                }
                catch (Exception ex)
                {
                    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, config) =>
            {
               //Add configuration here if you need!!
            }).UseStartup<Startup>();
    }
}
