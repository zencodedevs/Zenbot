using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Zenbot.BotCore.Interactions;

namespace Zenbot.BotCore
{
    /// <summary>
    /// The main class which is taking care of each interaction commands
    /// </summary>
    public class InteractionsHandler
    {
        private readonly IServiceProvider services;
        private readonly InteractionService _interactions;
        private readonly DiscordSocketClient _client;
        private readonly BotConfiguration _config;
        public InteractionsHandler(IServiceProvider services)
        {
            this.services = services;
            this._interactions = services.GetRequiredService<InteractionService>();
            this._client = services.GetRequiredService<DiscordSocketClient>();
            this._config = services.GetRequiredService<BotConfiguration>();
        }
        public async Task InitializeAsync()
        {
            await _interactions.AddModulesAsync(Assembly.GetExecutingAssembly(), this.services);

            _client.Ready += _client_Ready;
            async Task _client_Ready()
            {
                if (DiscordBotService.IsDebug())
                    await _interactions.RegisterCommandsToGuildAsync(_config.MainGuildId, true);
                else
                    await _interactions.RegisterCommandsGloballyAsync(true);
            }

            _client.InteractionCreated += _client_IntegrationCreated;
            _interactions.InteractionExecuted += _interactions_InteractionExecuted;
        }
        private async Task _client_IntegrationCreated(SocketInteraction interaction)
        {
            var ctx = new CustomSocketInteractionContext(this._client, interaction, interaction.Channel);
            await _interactions.ExecuteCommandAsync(ctx, this.services);
        }
        private async Task _interactions_InteractionExecuted(ICommandInfo info, Discord.IInteractionContext context, IResult result)
        {
            if (result.IsSuccess)
                return;

            if (context.Interaction.Type == Discord.InteractionType.ApplicationCommandAutocomplete)
                return;

            if (!context.Interaction.HasResponded)
            {
                await context.Interaction.RespondAsync(result.ErrorReason, ephemeral: true);
                return;
            }
            else
            {
                await context.Interaction.FollowupAsync(result.ErrorReason, ephemeral: true);
                return;
            }
        }
    }
}