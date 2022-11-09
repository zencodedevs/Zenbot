﻿using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Services
{
    public class BotUserGuildServices : EntityBaseService<BotUserGuild>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _discord;
        public BotUserGuildServices(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }


        // Get Or insert the current Guild from/to bot database
        public async Task<BotUserGuild> GetOrAddAsync(int guildId, int botUserId, bool isAdmin = false)
        {
            var userGuild = await base.GetAsync(x => x.BotUserId == botUserId && x.GuildId == guildId && x.IsAdmin == isAdmin);

            if (userGuild is null)
            {
                var newUserGuild = new BotUserGuild()
                {
                    GuildId = guildId,
                    BotUserId = botUserId,
                    IsAdmin = isAdmin
                };
                userGuild = await base.InsertAsync(newUserGuild);
            }
            return userGuild;
        }



        // Get users by Guild Id
        public async Task<List<BotUserGuild>> GetUsersByGuildId(int guildId)
        {
            return await base.GetManyAsync(x => x.GuildId == guildId);
        }

    }
}
