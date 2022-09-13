using Zenbot.Configuration;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Zenbot.Services
{
    /**
     * Listens to received messages to catch commands, sends commands over to Command Service & processes subsequent Command Service output
     */
    public class CommandHandlingService
    {
        private readonly IServiceProvider _services;
        private readonly IOptions<CommandsConfiguration> _commandsConfig;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly string _prefix;

        public CommandHandlingService(IServiceProvider services, IOptions<CommandsConfiguration> commandsConfig, CommandService commands, DiscordSocketClient client)
        {
            _services = services;
            _commandsConfig = commandsConfig;
            _commands = commands;
            _client = client;

            _prefix = _commandsConfig.Value.Prefix; // command prefix, such as ! or ~

            // Hook into CommandExecuted event to print out / log command execution result
            _commands.CommandExecuted += CommandExecutedAsync;

            // Hook into MessageReceived event to execute commands received as messages
            _client.MessageReceived += MessageReceivedAsync;
        }

        /**
         * Async part of CommandHandler initialisation 
         */
        public async Task InitializeAsync()
        {
          
            // Registers commands: all modules that are public and inherit ModuleBase<T>
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        // Take actions upon receiving messages
        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ensures we don't process system messages / messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;

            // Additional check that the message is not system / bot / webhook
            if (message.Source != MessageSource.User) return;

            var prefixOffset = 0;
            var context = new SocketCommandContext(_client, message);

            // HasStringPrefix checks for prefix at the start of received message and adjusts offset accordingly
            // Currently if prefix is empty - we process all messages as commands
            if (!String.IsNullOrEmpty(_prefix) && !message.HasStringPrefix(_prefix, ref prefixOffset))
                return;

            // Executes command if one is found that matches message context
            await _commands.ExecuteAsync(context, prefixOffset, _services);
        }

        /**
         * Handles result of command execution, including when command was not found
         */
        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // If a command isn't found, log that info to console and exit this method
            // Right now will log for every message without command
            if (!command.IsSpecified)
            {
                Console.WriteLine($"Command failed to execute for [{context.User.Username}], error message: [{result.ErrorReason}]");
                return;
            }

            // Log success to the console and exit this method
            if (result.IsSuccess)
            {
                Console.WriteLine($"Command [{command.Value.Name}] executed for [{context.User.Username}]");
                return;
            }

            // Remaining scenarios assume Failure; let the user know
            await context.Channel.SendMessageAsync($"Command [{command.Value.Name}] failed for {context.User.Username}, context: [{result}]");
        }
    }
}