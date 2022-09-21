using BotCore.Entities.BotCore;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BotCore
{
    public class DiscordBotService
    {
        //private readonly IServiceProvider _services;
        //public DiscordBotService(IServiceProvider services)
        //{
        //    this._services = services;
        //}


        public DiscordBotService ConfigServices(IServiceCollection services)
        {
            var configuration = BotConfiguration.GetConfiguration();

            services
                .AddSingleton(configuration)
                .AddSingleton<DiscordSocketClient>(x => new DiscordSocketClient(DiscordSocketConfig))

                .AddSingleton<InteractionService>(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionsHandler>()

                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()

                .AddSingleton<UsersService>()

                .AddSingleton<BrithdayService>()
                .AddSingleton<EventService>();

            return this;
        }

        public async Task RunAsync(IServiceProvider _services)
        {
            await Log("DiscordBotService", "Run Async");

            var config = _services.GetRequiredService<BotConfiguration>();

            var client = _services.GetRequiredService<DiscordSocketClient>();
            client.Log += Client_Log;

            await _services.GetRequiredService<InteractionsHandler>().InitializeAsync();
            await _services.GetRequiredService<CommandHandler>().InitializeAsync();

            var brithday = _services.GetRequiredService<BrithdayService>();
            var events = _services.GetRequiredService<EventService>();


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