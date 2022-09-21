using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;


namespace Zenbot.BotCore
{
    public class CommandHandler
    {
        private readonly IServiceProvider services;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly BotConfiguration _botConfiguration;
        public CommandHandler(IServiceProvider services)
        {
            this.services = services;

            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _botConfiguration = services.GetRequiredService<BotConfiguration>();

        }
        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), this.services);
            _client.MessageReceived += _client_MessageReceived;
            _commands.CommandExecuted += _commands_CommandExecuted;
        }

        private async Task _client_MessageReceived(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message))
                return;
            if (message.Source != Discord.MessageSource.User)
                return;

            var argPos = 0;
            if (!message.HasStringPrefix(_botConfiguration.Prefix, ref argPos))
                return;

            var ctx = new SocketCommandContext(this._client, message);
            var result = await _commands.ExecuteAsync(ctx, argPos, this.services);

        }
        private async Task _commands_CommandExecuted(Discord.Optional<CommandInfo> info, ICommandContext context, IResult result)
        {
            if (result.IsSuccess)
                return;

            if (!info.IsSpecified)
            {
                await context.Channel.SendMessageAsync("command not found !");
                return;
            }

            await context.Channel.SendMessageAsync(
                $"**An Error Occurred !**" +
                $"\nError Reason: {result.ErrorReason}");
        }
    }
}