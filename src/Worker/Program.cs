using Application.Shared;
using Domain.Shared;
using Infrastructure.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using NLog;
using Quartz;
using Microsoft.Extensions.Logging;
using ZenAchitecture.Domain.Shared.Common;
using ZenAchitecture.Domain.Shared.Interfaces;
using NLog.Web;
using Worker.Services;
using Worker.Jobs;
using Worker.Extensions;

namespace Worker
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    var env = hostContext.HostingEnvironment;

                    configBuilder
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();
                })
                .ConfigureServices((hostContext, services) =>
                {

                    Configuration = hostContext.Configuration;

                    // set nlog connection string
                    GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("DefaultConnection"));

                    //set nlog inster clause variable
                    LogManager.Configuration.Variables["registerClause"] = Constants.Nlog.WorkerDbRegisterClause;

                    services.AddSingleton<ICurrentUserService, CurrentUserPuppeteerService>();

                    // add microsoft feature managment
                    services.AddFeatureManagement();

                    services.AddApplicationShared(Configuration);

                    services.AddDomainShared();

                    services.AddInfrastructureShared(Configuration);

                    // Add the required Quartz.NET services
                    services.AddQuartz(quartz =>
                    {
                        // Use a Scoped container to create jobs. I'll touch on this later
                        quartz.UseMicrosoftDependencyInjectionScopedJobFactory();

                        // Register the job, loading the schedule from configuration
                        quartz.AddJobTrigger<EventProcessorQuartezService>(hostContext.Configuration);
                        quartz.AddJobTrigger<EventProcessorCleanerQuartezService>(hostContext.Configuration);

                    });

                    // Add the Quartz.NET hosted service

                    services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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
