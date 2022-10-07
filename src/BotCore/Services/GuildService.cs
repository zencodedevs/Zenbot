using BotCore.Entities;
using BotCore.Utilities;
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
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Infrastructure.Repositories;
using Zen.Uow;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Entities.Bot.Dtos.BitbucketWebHook;

namespace BotCore.Services
{
    public class GuildService : EntityBaseService<Guild>
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;
        private readonly BotConfiguration _config;
        private readonly ChannelService _channelService;
        public GuildService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
            _channelService = _services.GetRequiredService<ChannelService>();
            _config = _services.GetRequiredService<BotConfiguration>();
        }
        public async Task<Guild> UpdateGuildAsync(int id, Action<Guild> value)
        {
            return await base.UpdateAsync(id, value);
        }

        public async Task<GuildChannel> GetChannelAsync(int guildId, GuildChannelType channelType)
        {
            return await _channelService.GetAsync(a => a.GuildId == guildId && a.Type == channelType);
        }
        public async Task<Guild> GetOrAddAsync(ulong id)
        {
            var guild = await base.GetAsync(x => x.GuildId == id);

            if (guild is null)
            {
                var newGuild = new Guild()
                {
                    GuildId = id,
                    BotPrefix = _config.Prefix
                };
                guild = await base.InsertAsync(newGuild);
            }
            return guild;
        }
        public async Task SyncAllGuildsUsersRolesAsync()
        {
            var guilds = await GetManyAsync(x => x.VerifiedRoleId != 0 && x.UnVerifiedRoleId != 0);
            foreach (var guild in guilds)
            {
                await SyncGuildUsersRolesAsync(guild);
                await Task.Delay(TimeSpan.FromSeconds(2));
            }
        }
        public async Task SyncGuildUsersRolesAsync(Guild guild)
        {
            var discordGuild = _discord.GetGuild(guild.GuildId);

            var users = await GuildRolesManagment.GetUsersWithoutAnyRoleAsync(discordGuild, guild.VerifiedRoleId, guild.UnVerifiedRoleId);
            await GuildRolesManagment.SyncMemberRolesAsync(discordGuild, users, guild.UnVerifiedRoleId);
        }
    }
}
