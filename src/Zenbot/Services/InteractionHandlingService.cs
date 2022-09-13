using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;



namespace Zenbot.Services
{
    public class InteractionHandlingService
    {
        private readonly DiscordSocketClient _discordSocketClient; // Presumably this should be same client
                                                                   // as the one used for InteractionService
                                                                   // initialisation; TBU: change up the code
                                                                   // so that client type is specified in one
                                                                   // place for both InteractionService & 
                                                                   // InteractionHandlingService
        private readonly InteractionService _interactionHandler;
        private readonly IServiceProvider _services;

        public InteractionHandlingService(DiscordSocketClient discordSocketClient, InteractionService interactionHandler, IServiceProvider services)
        {
            _discordSocketClient = discordSocketClient;
            _interactionHandler = interactionHandler;
            _services = services;

            _discordSocketClient.Ready += ReadyAsync;
            _interactionHandler.Log += LogAsync;
            _discordSocketClient.InteractionCreated += HandleInteraction;
        }

        public async Task InitializeAsync()
        {
            // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService
            await _interactionHandler.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task LogAsync(LogMessage log)
            => Console.WriteLine(log);

        private async Task ReadyAsync()
        {
            // If any new slash commands are to be registered - it has to be after client is ready

            // TBU: Add optional code for adding a new command
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
            var context = new SocketInteractionContext(_discordSocketClient, interaction);

            // Execute the incoming command.
            var result = await _interactionHandler.ExecuteCommandAsync(context, _services);

            if (!result.IsSuccess)
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // TBU: implement different handling for different types of Errors
                        break;
                    default:
                        await interaction.RespondAsync(result.Error.ToString());
                        break;
                }
        }
    }

}

