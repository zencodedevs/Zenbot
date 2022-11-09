using BotCore.Entities;
using BotCore.Handlers;
using BotCore.Services;
using BotCore.Services.Birthday;
using BotCore.Services.Bitbucket;
using BotCore.Services.Jira;
using BotCore.Services.Locale;
using BotCore.Services.ScrinIO;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Zen.Uow;

namespace BotCore
{
    /// <summary>
    ///        The main Class for Zenbot
    /// </summary>
    public class BotService
    {
        private readonly IWebHostEnvironment _env;

        public BotService(IWebHostEnvironment env)
        {
            _env = env;
        }
        public BotService ConfigServices(IServiceCollection services)
        {
            var configuration = BotConfiguration.GetConfiguration(_env);

            services
                .AddSingleton(configuration)
                .AddSingleton(x => new DiscordSocketClient(DiscordSocketConfig))

                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionsHandler>()

                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()

                .AddSingleton<UserService>()
                .AddSingleton<GuildService>()
                .AddSingleton<ChannelService>()
                .AddSingleton<BirthdayMessageService>()
                .AddSingleton<WelcomeMessageService>()
                .AddSingleton<SupervisorService>()
                .AddSingleton<VocationService>()
                .AddSingleton<BotUserGuildServices>()

                .AddSingleton<JiraService>()
                .AddSingleton<BitbucketService>()
                .AddSingleton<LocaleService>()

                .AddSingleton<ScrinIOService>()
                .AddSingleton<GsuiteServices>()

                .AddSingleton<BirthdayService>()
                .AddSingleton<EventService>();

            return this;
        }

        public async Task RunAsync(IServiceProvider _services)
        {
            await Log("BotService", "Run Async");

            var config = _services.GetRequiredService<BotConfiguration>();

            await _services.GetRequiredService<LocaleService>().InitializeAsync();

            var client = _services.GetRequiredService<DiscordSocketClient>();
            client.Log += Client_Log;

            await _services.GetRequiredService<InteractionsHandler>().InitializeAsync();
            await _services.GetRequiredService<CommandHandler>().InitializeAsync();

            var brithday = _services.GetRequiredService<BirthdayService>();
            var events = _services.GetRequiredService<EventService>();
            var scrinio = _services.GetRequiredService<ScrinIOService>();
            var gsuite = _services.GetRequiredService<GsuiteServices>();


            await client.LoginAsync(TokenType.Bot, config.BotToken, true);
            await client.StartAsync();
        }

        private Task Client_Log(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        public DiscordSocketConfig DiscordSocketConfig => new DiscordSocketConfig()
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers
        };

        public static Task Log(string service, string content, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(DateTime.UtcNow.ToString("T") + " " + service.PadRight(10) + content);
            return Task.CompletedTask;
        }

        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
return false;
#endif
        }
    }
}