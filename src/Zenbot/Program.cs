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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Infrastructure.Repositories;
using Zen.Uow;
using ZenAchitecture.Domain.Shared.Common;
using ZenAchitecture.Domain.Shared.Interfaces;
using Zenbot.Modules.Birthday;
using Zenbot.Services;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace Zenbot
{

    /// <summary>
    /// The entry point of the bot.
    /// </summary>
    /// 

    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private Timer timer;
   


        public static IConfiguration Configuration { get; private set; }

        static void Main(string[] args)
        {
           

            new Program().RunBothAsync(args).GetAwaiter().GetResult();
        }


        public async Task RunBothAsync(string[] args)
        {
            var _builder = new ConfigurationBuilder()
               .SetBasePath(AppContext.BaseDirectory)
               .AddJsonFile(path: "appsettings.json");


          //  var env = hostContext.HostingEnvironment;

              var c = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

         

           // CreateHostBuilder(args).Build();

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All | GatewayIntents.GuildMembers
            });

            _commands = new CommandService();
            _services = new ServiceCollection()
                        .AddSingleton(_client)
                        .AddSingleton(_commands)
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

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            var token = "MTAxODc2OTI1OTI1MTM4ODQ1Nw.GjdhCz.UwzzqacE59vaM4gbvpM3JY2EQ4SuIYbgqd7Fog";

            _client.Log += Log;

            _client.UserJoined += AnnounceJoinedUser;


            _client.MessageReceived += HandleCommandsAsync;

            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            timer = new Timer(TimedAnnouncement, null, 0, 6000); // 24 hour interval

            await Task.Delay(-1);




        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        // Triggers the daily check/announcement of any existing birthdays.
        public async void TimedAnnouncement(object state)
        {
            if (DateTime.Now.Hour <= 8)
                await AnnounceBirthdays();
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user) //Welcomes the new user
        {
            var channel = _client.GetChannel(1018765311215947816) as SocketTextChannel; // Gets the channel to send the message in
            await channel.SendMessageAsync($"Welcome {user.Username} to {channel.Guild.Name}"); //Welcomes the new user

        }
        public  Task AnnounceBirthdays()
        {
            //Database.Birthday[] birthdays = Data.Data.GetBirthdays();
            var builder = new EmbedBuilder()
                .WithTitle("Happy Birthday")
                .WithColor(new Color(0x8CE3C5))
                .WithImageUrl("https://media.giphy.com/media/xUOxf0vukEHTKkD4ic/giphy.gif");
            var channel = _client.GetGuild(1018765173969932319).GetChannel(1018765311215947816) as SocketTextChannel;
            int numOfBirthdays = 0;
            DateTime today = DateTime.Now;
            //var channel = _client.GetGuild(1018765173969932319).GetChannel(1018765311215947816) as IMessageChannel;

            string bdayMessage = "Happy Birthday to You";

            //foreach (Birthday bday in birthdays)
            //{
            //    if (bday.Month == today.Month && bday.Day == today.Day)
            //    {
            //numOfBirthdays++;
            //bdayMessage += $" {Client.GetUser(bday.UserId).Mention}";
            //    }
            //}

            // No announcement will be made if there are no birthdays.
            channel.SendMessageAsync($"{bdayMessage}!", false, builder.Build());

            return Task.CompletedTask;
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

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //   Host.CreateDefaultBuilder(args)
        //       .ConfigureAppConfiguration((hostContext, configBuilder) =>
        //       {
        //           var env = hostContext.HostingEnvironment;

        //           configBuilder
        //               .SetBasePath(env.ContentRootPath)
        //               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //               .AddEnvironmentVariables()
        //               .Build();
        //       })
        //       .ConfigureServices((hostContext, services) =>
        //       {

        //           Configuration = hostContext.Configuration;

        //           // set nlog connection string
        //           GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("DefaultConnection"));

        //           //set nlog inster clause variable
        //           LogManager.Configuration.Variables["registerClause"] = Constants.Nlog.ZenBotDbRegisterClause;

        //         //  services.AddSingleton<ICurrentUserService, CurrentUserPuppeteerService>();

        //         //  services.AddApplicationShared(Configuration);

        //          // services.AddDomainShared();

        //          // services.AddInfrastructureShared(Configuration);

        //       })
        //       .ConfigureLogging(logging =>
        //       {
        //           /* Clean providers */
        //           logging.ClearProviders();
        //           /* Set minimum log level*/
        //           logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
        //       })
        //       .UseNLog();
    }
}




