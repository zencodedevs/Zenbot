using Discord;
using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Uow;
using Zenbot.Domain.Shared.Entities.Bot.Dtos.JiraWebHook;

namespace BotCore.Services
{

    /// <summary>
    /// User data Interaction with database 
    /// </summary>
    public class UserService : EntityBaseService<BotUser>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _discord;
        public UserService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }

        public async Task<IMessage> SendMessageToUserAsync(ulong userId, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed[] embeds = null)
        {
            IUser discordUser = await _discord.Rest.GetUserAsync(userId);
            if (discordUser == null) return null;

            return await discordUser.SendMessageAsync(text, isTTS, embed, options, allowedMentions, components, embeds);
        }
        public async Task<List<BotUser>> GetUpComingUsersBrithday()
        {
            var TodayMonth = DateTime.UtcNow.Month;
            var TodayDay = DateTime.UtcNow.Day;

            return await base.GetManyAsync(x => x.Birthday.Month == TodayMonth && x.Birthday.Day == TodayDay);
        }
        public async Task<List<BotUser>> GetUsersBrithday()
        {
            return await base.GetManyAsync(a => a.Birthday != DateTime.MinValue);
        }

        public async Task<List<BotUser>> GetUsersOfGuild(int guildId)
        {
            return await base.GetManyAsync(a => a.GuildId == guildId);
        }

        public async Task<BotUser> GetOrAddAsync(ulong Id, int guildId)
        {
            var user = await base.GetAsync(a => a.DiscordId == Id);
            if (user == null)
            {
                user = new BotUser()
                {
                    DiscordId = Id,
                    GuildId = guildId
                };
                user = await base.InsertAsync(user);
            }
            return user;
        }
        public async Task<BotUser> GetUserByJiraId(string jiraId)
        {
            return await base.GetAsync(x => x.JiraAccountID == jiraId);
        }

        public async Task<BotUser> GetUserByBitbucketId(string bitbucketId)
        {
            return await base.GetAsync(x => x.BitBucketAccountId == bitbucketId);
        }
    }
}