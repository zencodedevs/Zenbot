namespace   Worker.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Quartz;
    using System;

    public static class QuartzConfigurationExtensions
    {
        public static void AddJobTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config) where T : IJob
        {
            // Use the name of the IJob as the appsettings.json key
            string serviceName = typeof(T).Name;

            // Try and load the schedule from configuration
            var configurationKey = $"Quartz:{serviceName}";

            var cronSchedule = config[configurationKey];

            // Some minor validation
            if (string.IsNullOrEmpty(cronSchedule))
                throw new Exception($"No Quartz.NET Cron schedule found for job in configuration at {configurationKey}");


            // register the job as before
            var jobKey = new JobKey(serviceName);
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));

            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(serviceName + "-Trigger")
                .WithCronSchedule(cronSchedule)); // use the schedule from configuration
        }
    }
}
