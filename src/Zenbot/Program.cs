using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Threading.Tasks;
using Discord.Interactions;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.Extensions.Configuration.Yaml;
using ZenAchitecture.Domain.Shared.Common;
using NLog.Web;
using Zenbot.Modules.Birthday;
using System.Threading;
using System.Reflection;
using Domain.Shared;
using ZenAchitecture.Domain.Shared.Interfaces;
using Zenbot.Services;
using Microsoft.AspNetCore.Http;
using Application.Shared;
using Infrastructure.Shared;

namespace Zenbot
{

    /// <summary>
    /// The entry point of the bot.
    /// </summary>
    /// 

    public class Program
    {
        private readonly IConfiguration _config;
        private DiscordSocketClient _client;
        private InteractionService _commands;
        private ulong _testGuildId;

        private Timer timer;



        public static IConfiguration Configuration { get; private set; }

       
        public static Task Main(string[] args) => new Program().MainAsync();
       

        public Program()
        {
            // create the configuration
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");

            // build the configuration and assign to _config          
            _config = _builder.Build();
            _testGuildId = ulong.Parse(_config["GuildId"]);

            var _builder2 = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile(path: "appsettings.json");

        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All | GatewayIntents.GuildMembers
            });

            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                var commands = services.GetRequiredService<InteractionService>();

               


                // setup logging and the ready event
                client.Log += LogAsync;
                commands.Log += LogAsync;
                client.Ready += ReadyAsync;

                client = new DiscordSocketClient(new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.All | GatewayIntents.GuildMembers
                });

                client.UserJoined += AnnounceJoinedUser;

                _client = client;
                _commands = commands;

                await client.LoginAsync(TokenType.Bot, _config["Token"]);
                await client.StartAsync();

                timer = new Timer(TimedAnnouncement, null, 0, 3000000000); // 24 hour interval

                // we get the CommandHandler class here and call the InitializeAsync method to start things up for the CommandHandler service
                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(Timeout.Infinite);
            }
        }

       

        private ServiceProvider ConfigureServices()
        {
            var c = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            return new ServiceCollection()
                        .AddSingleton(_config)
                        .AddSingleton<DiscordSocketClient>()
                        .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                        .AddSingleton<CommandHandler>()
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

        }

        private async Task ReadyAsync()
        {
            
                // this method will add commands globally, but can take around an hour
                await _commands.RegisterCommandsGloballyAsync(true);
            
            Console.WriteLine($"Connected as -> [{_client.CurrentUser}] :)");
        }


        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }


        public async void TimedAnnouncement(object state)
        {
            var datae = DateTime.Now.Hour;
            if (DateTime.Now.Minute == 36)
                await Announce.AnnounceBirthdays(_client);
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user) 
        {
            var channel = _client.GetChannel(1018765311215947816) as SocketTextChannel; 
            await channel.SendMessageAsync($"Welcome {user.Username} to {channel.Guild.Name}"); 

        }





        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureServices((hostContext, services) =>
               {

                   Configuration = hostContext.Configuration;

                   // set nlog connection string
                   GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("DefaultConnection"));

                   //set nlog inster clause variable
                   LogManager.Configuration.Variables["registerClause"] = Constants.Nlog.ZenBotDbRegisterClause;

               })
               .ConfigureLogging(logging =>
               {
                   /* Clean providers */
                   logging.ClearProviders();
                   /* Set minimum log level*/
                   logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
               })
               .UseNLog();


        static bool IsDebug()
        {
#if DEBUG
            return true;
#else
                return false;
#endif
        }
    }
}




