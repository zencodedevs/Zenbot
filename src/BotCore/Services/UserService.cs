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
using Zenbot.Domain.Shared.Entities.Bot;
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

        // Send message directly to user
        public async Task<IMessage> SendMessageToUserAsync(ulong userId, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed[] embeds = null)
        {
            IUser discordUser = await _discord.Rest.GetUserAsync(userId);
            if (discordUser == null) return null;

            return await discordUser.SendMessageAsync(text, isTTS, embed, options, allowedMentions, components, embeds);
        }

        // check list of all upcomming birthdays
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

        // List of all supervisors for this Guild
        public async Task<List<BotUser>> GetSupervisorersForCurrentGuild(List<BotUserGuild> botUserGuilds)
        {
            var sprs = new List<BotUser>();
            foreach (var item in botUserGuilds)
            {
                var user= await base.GetAsync( x => x.Id == item.BotUserId && x.IsSupervisor);
                if(user!= null) sprs.Add(user);
            }
            return sprs;
        }

        // User by discord Id
        public async Task<BotUser> GetUserByDiscordId(ulong discordId)
        {
            return await base.GetAsync(a => a.DiscordId == discordId);
        }


        // Add user to database by first interaction with bot
        public async Task<BotUser> GetOrAddAsync(ulong Id, string username)
        {
            var user = await base.GetAsync(a => a.DiscordId == Id);
            if (user == null)
            {
                user = new BotUser()
                {
                    DiscordId = Id,
                    Username = username
                };
                user = await base.InsertAsync(user);
            }
            return user;
        }


        // birthday
        public async Task<BotUser> GetOrAddAsync(ulong Id, string username, DateTime birthday)
        {
            var user = await base.GetAsync(a => a.DiscordId == Id);
            if (user == null)
            {
                user = new BotUser()
                {
                    DiscordId = Id,
                    Username = username,
                    Birthday = birthday
                };
                user = await base.InsertAsync(user);
            }
            else
            {
               user = await base.UpdateAsync(user.Id, x => {
                    x.Birthday = birthday;
                });
            }
            return user;
        }

        // INtegration
        public async Task<BotUser> GetOrAddAsync(ulong Id, string username, string jiraAccount, string bitBucketAccount)
        {
            var user = await base.GetAsync(a => a.DiscordId == Id);
            if (user == null)
            {
                user = new BotUser()
                {
                    DiscordId = Id,
                    Username = username,
                    JiraAccountID = jiraAccount,
                    BitBucketAccountId = bitBucketAccount
                };
                user = await base.InsertAsync(user);
            }
            else
            {
                user = await base.UpdateAsync(user.Id, x => {
                    x.JiraAccountID = jiraAccount;
                    x.BitBucketAccountId = bitBucketAccount;
                });
            }
            return user;
        }

        // Get user by Jira account id
        public async Task<BotUser> GetUserByJiraId(string jiraId)
        {
            return await base.GetAsync(x => x.JiraAccountID == jiraId);
        }

        // Get user by bitbucket account Id
        public async Task<BotUser> GetUserByBitbucketId(string bitbucketId)
        {
            return await base.GetAsync(x => x.BitBucketAccountId == bitbucketId);
        }

        // Get user by ID
        public async Task<BotUser> GetUserById(int Id)
        {
            return await base.GetAsync(x => x.Id == Id);
        }
    }
}