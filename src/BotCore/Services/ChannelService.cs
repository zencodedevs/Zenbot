using BotCore.Entities;
using Discord;
using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Zen.Common.Extensions;
using Zen.Domain.Interfaces;
using Zen.Uow;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Services
{
    public class ChannelService : EntityBaseService<GuildChannel>
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;
        //private readonly BotConfiguration _config;

        public ChannelService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }

        // Common method for sending message to logger channel
        public async Task SendMessageAsync(ulong channelId, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null, MessageComponent components = null, ISticker[] stickers = null, Embed[] embeds = null, MessageFlags flags = MessageFlags.None)
        {
            var channel = (ITextChannel)(_discord.GetChannel(channelId) ?? await _discord.GetChannelAsync(channelId));
            await channel.SendMessageAsync(text, isTTS, embed, options, allowedMentions, messageReference, components, stickers, embeds, flags);
        }


        // methodf to Get Or Add a logger channel for/from this Guild
        public async Task<GuildChannel> GetOrAddAsync(ulong id, int guildId)
        {
            var channel = await base.GetAsync(a => a.ChannelId == id);
            if (channel is null)
            {
                var @new = new GuildChannel()
                {
                    ChannelId = id,
                    GuildId = guildId,
                    Type = Zenbot.Domain.Shared.Entities.Bot.GuildChannelType.None
                };
                channel = await base.InsertAsync(@new);
            }
            return channel;
        }
    }
}
