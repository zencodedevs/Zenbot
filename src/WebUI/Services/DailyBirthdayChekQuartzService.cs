using BotCore.Services;
using BotCore.Services.Birthday;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Zenbot.WebUI.Services
{
    public class DailyBirthdayChekQuartzService : IJob
    {
        private readonly IServiceProvider _services;
        private readonly UserService _userService;
        private readonly BirthdayService _birthdayService;
        private readonly IFeatureManager _featureManager;

        public DailyBirthdayChekQuartzService(IServiceProvider services, UserService userService, BirthdayService birthdayService, IFeatureManager featureManager)
        {
            _services = services;
            _userService = services.GetRequiredService<UserService>();
            _birthdayService = services.GetRequiredService<BirthdayService>();
            _featureManager = featureManager;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            //if (await _featureManager.IsEnabledAsync("DailyBirthdayChekQuartzService"))
            //{
                var users = await _userService.GetUpComingUsersBrithday();
                await _birthdayService.NotficationUsersBirthdayAsync(users);
                Console.WriteLine("job is runing...");
            //}
        }
    }
}
