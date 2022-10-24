using BotCore.Entities;
using BotCore.Services;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;


namespace BotCore.Handlers
{
    /// <summary>
    /// The main class which is taking care of each interaction commands
    /// </summary>
    public class InteractionsHandler
    {
        private readonly IServiceProvider services;
        private readonly InteractionService _interactions;
        private readonly DiscordSocketClient _discord;
        private readonly BotConfiguration _config;

        private readonly GuildService _guildService;
        private readonly ChannelService _channelService;
        private readonly UserService _userService;
        public InteractionsHandler(IServiceProvider services)
        {
            this.services = services;
            _interactions = services.GetRequiredService<InteractionService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _config = services.GetRequiredService<BotConfiguration>();

            _guildService = services.GetRequiredService<GuildService>();
            _channelService = services.GetRequiredService<ChannelService>();
            _userService = services.GetRequiredService<UserService>();
        }
        public async Task InitializeAsync()
        {
            await _interactions.AddModulesAsync(Assembly.GetExecutingAssembly(), services);

            _discord.Ready += _client_Ready;
            async Task _client_Ready()
            {
                //if (BotService.IsDebug())
                //    await _interactions.RegisterCommandsToGuildAsync(1018765173969932319, true);
                //else
                    await _interactions.RegisterCommandsGloballyAsync(true);
            }

            _discord.InteractionCreated += _client_IntegrationCreated;
            _interactions.InteractionExecuted += _interactions_InteractionExecuted;
        }
        private async Task _client_IntegrationCreated(SocketInteraction interaction)
        {
            var ctx = new CustomSocketInteractionContext(_discord, interaction, _guildService, _channelService, _userService, interaction.Channel);
            await _interactions.ExecuteCommandAsync(ctx, services);
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