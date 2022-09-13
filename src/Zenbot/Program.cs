using Application.Shared;
using Application.Shared.Services;
using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Domain.Shared;
using Domain.Shared.Interfaces;
using Infrastructure.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using NLog;
using NLog.Web;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Infrastructure.Repositories;
using Zen.Uow;
using ZenAchitecture.Domain.Shared.Common;
using ZenAchitecture.Domain.Shared.Interfaces;
using Zenbot.Services;

using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Zenbot
{

    /// <summary>
    /// The entry point of the bot.
    /// </summary>
    /// 

    public class Constant
    {
        public static readonly string _birthdayDateFormat = "dd MMM";
    }

    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;




        public static IConfiguration Configuration { get; private set; }

        static void Main(string[] args)
        {
            new Program().RunBothAsync(args).GetAwaiter().GetResult();
        }


        public async Task RunBothAsync(string[] args)
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All | GatewayIntents.GuildMembers
            });

            _commands = new CommandService();
            _services = new ServiceCollection()
                        .AddSingleton(_client)
                        .AddSingleton(_commands)
                        .AddSingleton<IUnitOfWork, UnitOfWork>()
                        .AddSingleton<ITest, Test>()
                        
                        .BuildServiceProvider();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            var token = "Token Key";

            _client.Log += Log;

            _client.UserJoined += AnnounceJoinedUser;


            _client.MessageReceived += HandleCommandsAsync;

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);

            CreateHostBuilder(args).Build();


        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }


        public async Task AnnounceJoinedUser(SocketGuildUser user) //Welcomes the new user
        {
            var channel = _client.GetChannel(1018765311215947816) as SocketTextChannel; // Gets the channel to send the message in
            await channel.SendMessageAsync($"Welcome {user.Username} to {channel.Guild.Name}"); //Welcomes the new user

        }


        private async Task HandleCommandsAsync(SocketMessage arg)
        {
            if (arg is SocketUserMessage message)
            {
                var context = new SocketCommandContext(_client, message);
                if (context != null)
                {
                    if (message.Author.IsBot) return;

                    int argPos = 0;

                    if (message.HasStringPrefix("!", ref argPos))
                    {
                        var result = await _commands.ExecuteAsync(context, argPos, _services);

                        if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                    }
                }
            }

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
                   LogManager.Configuration.Variables["registerClause"] = Constants.Nlog.ZenBotDbRegisterClause;

                   services.AddSingleton<ICurrentUserService, CurrentUserPuppeteerService>();

                   // add microsoft feature managment
                   services.AddFeatureManagement();

                   services.AddApplicationShared(Configuration);

                   services.AddDomainShared();

                   services.AddInfrastructureShared(Configuration);

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

