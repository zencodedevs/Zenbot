using BotCore;
using BotCore.Services.Birthday;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Quartz;
using System;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities;
using Zenbot.Infrastructure.Persistence;
using Zenbot.Infrastructure.Shared.Persistence;
using Zenbot.WebUI.Extensions;
using Zenbot.WebUI.Services;

namespace Zenbot.WebUI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args)
            .ConfigureAppConfiguration((hostContext, builder) =>
            {
                // Add other providers for JSON, etc.

                if (hostContext.HostingEnvironment.IsDevelopment())
                {
                    builder.AddUserSecrets<Program>();
                }
            })
            .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    if (context.Database.IsSqlServer())
                    {
                        context.Database.Migrate();
                    }

                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);

                    await ApplicationDbContextSeed.SeedDefaultDataAsync(context);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                    logger.LogError(ex, "An error occurred while migrating or seeding the database.");

                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    })
                .ConfigureServices((hostContext, services) =>
                 {


                     // Add the required Quartz.NET services
                     services.AddQuartz(quartz =>
                     {
                         // Use a Scoped container to create jobs. I'll touch on this later
                         quartz.UseMicrosoftDependencyInjectionScopedJobFactory();

                         // Register the job, loading the schedule from configuration
                         quartz.AddJobTrigger<DailyBirthdayChekQuartzService>(hostContext.Configuration);
                     });

                     // Add the Quartz.NET hosted service

                     services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

                     // other config
                 })
                 .ConfigureLogging(logging =>
                 {
                     /* Clean providers */
                     logging.ClearProviders();
                     /* Set minimum log level*/
                     logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                 })
                .UseNLog();
    }
}
