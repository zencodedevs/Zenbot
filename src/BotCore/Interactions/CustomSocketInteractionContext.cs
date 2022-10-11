using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotCore;
using Zenbot.Domain.Shared.Entities.Bot;
using BotCore.Services;
using Microsoft.Extensions.Configuration;
using GuildChannel = Zenbot.Domain.Shared.Entities.Bot.GuildChannel;

namespace BotCore.Entities
{

    // Bot's custome SocketInteractionContext for more costomization
    public class CustomSocketInteractionContext : InteractionContext
    {
        public GuildService _guildService;
        public UserService _userService;
        public ChannelService _channelService;

        public CustomSocketInteractionContext(IDiscordClient client, IDiscordInteraction interaction, GuildService guildService, ChannelService channelService, UserService userService, IMessageChannel channel = null) : base(client, interaction, channel)
        {
            this._guildService = guildService;
            this._userService = userService;
            this._channelService = channelService;

        }


        // Here are some methods which will be called when bot is ready and during bot lifetime
        // We use from this methods which avoid too much request to our database


        public object Data { get; set; }

        private GuildChannel _botChannel;
        public GuildChannel GuildChannel
        {
            get
            {
                return GetBotChannelAsync(false).Result;
            }
        }

        // Getting logger channel for this Guild
        public async Task<GuildChannel> GetBotChannelAsync(bool refresh)
        {
            if (!Interaction.ChannelId.HasValue)
                throw new Exception("Channel Id can not be null.");

            if (_botChannel is null || refresh)
            {
                var guild = this.BotGuild;
                _botChannel = await _channelService.GetOrAddAsync(Interaction.ChannelId.Value, guild.Id);
            }
            return _botChannel;
        }


        // Current User using bot
        private BotUser _user;
        public BotUser BotUser
        {
            get
            {
                return GetBotUserAsync(false).Result;
            }
        }


        public async Task<BotUser> GetBotUserAsync(bool refresh)
        {
            if (_user is null || refresh)
            {
                _user = await _userService.GetOrAddAsync(Interaction.User.Id);
            }
            return _user;
        }



        // Current Guild bot is running in
        private Guild _botGuild;
        public Guild BotGuild
        {
            get
            {
                return GetBotGuildAsync(false).Result;
            }
        }
        public async Task<Guild> GetBotGuildAsync(bool refresh)
        {
            if (_botGuild == null || refresh)
            {
                _botGuild = await _guildService.GetOrAddAsync(Interaction.GuildId.Value);
            }
            return _botGuild;
        }

    }
}
