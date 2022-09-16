using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using System.Threading;
using Discord.Interactions;
using Discord.Commands;

namespace Zenbot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            await Log("Program", "Main Async");

            var configuration = BotConfiguration.GetConfiguration();

            var services = new ServiceCollection()
                .AddSingleton(configuration)
                .AddSingleton<DiscordSocketClient>(x => new DiscordSocketClient(DiscordSocketConfig))

                .AddSingleton<InteractionService>(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionsHandler>()

                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()

                .AddSingleton<UsersService>()

                .AddSingleton<EventService>()
                .BuildServiceProvider();

            await RunAsync(services);
        }
        public async Task RunAsync(IServiceProvider services)
        {
            var config = services.GetRequiredService<BotConfiguration>();

            var client = services.GetRequiredService<DiscordSocketClient>();
            client.Log += Client_Log;

            await services.GetRequiredService<InteractionsHandler>().InitializeAsync();
            await services.GetRequiredService<CommandHandler>().InitializeAsync();

            var events = services.GetRequiredService<EventService>();


            await client.LoginAsync(TokenType.Bot, config.BotToken, true);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite);
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

        public Task Log(string service, string content, ConsoleColor color = ConsoleColor.White)
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