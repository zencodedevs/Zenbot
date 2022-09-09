using Discord;
using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;


namespace Zenbot
{

    /// <summary>
    /// The entry point of the bot.
    /// </summary>
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;


        static void Main(string[] args) => new Program().RunBothAsync().GetAwaiter().GetResult();

        public async Task RunBothAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _services = new ServiceCollection().AddSingleton(_client)
                                               .AddSingleton(_commands).BuildServiceProvider();

            var token = "Tokey Key";

            _client.Log += Log;


            await RegisterCommandsAsync();


            await _client.LoginAsync(TokenType.Bot, token);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        public async Task RegisterCommandsAsync()
        {
            _client.UserJoined += AnnounceJoinedUser;
            _client.MessageReceived += HandleCommandsAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task AnnounceJoinedUser(SocketGuildUser user) //Welcomes the new user
        {
            var channel = _client.GetChannel(1016646322893377569) as SocketTextChannel; // Gets the channel to send the message in
            await channel.SendMessageAsync($"Welcome {user.Username} to {channel.Guild.Name}"); //Welcomes the new user
        }

        private async Task HandleCommandsAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
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
}

