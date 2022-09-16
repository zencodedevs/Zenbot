using Application.Shared;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.Shared;
using Infrastructure.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Worker.Services;
using ZenAchitecture.Domain.Shared.Interfaces;
using Zenbot.Entities.Zenbot;

namespace Zenbot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        public async Task MainAsync()
        {
            var c = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables()
           .Build();

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

                .AddSingleton<BrithdayService>()
                .AddSingleton<EventService>()

                //For connecting to database
                .AddSingleton<ICurrentUserService, CurrentUserPuppeteerService>()
                        .AddSingleton<IHttpContextAccessor, HttpContextAccessor>()

                        .AddTransient<IConfiguration>(sp =>
                        {
                            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
                            configurationBuilder.AddJsonFile("appsettings.json");
                            return configurationBuilder.Build();
                        })
                        .AddDomainShared()
                        .AddApplicationShared(c)
                        .AddInfrastructureShared(c)
                        

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

            var brithday = services.GetRequiredService<BrithdayService>();
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