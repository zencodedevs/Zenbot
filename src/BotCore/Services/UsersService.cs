﻿using Discord;
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

namespace BotCore
{

    /// <summary>
    /// User data Interaction with database 
    /// </summary>
    public class UsersService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _discord;
        public UsersService(IServiceProvider services, IServiceScopeFactory scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }

        public async Task<IMessage> TrySendMessageToUserAsync(ulong userId, string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed[] embeds = null)
        {
            IUser discordUser = await _discord.Rest.GetUserAsync(userId);
            if (discordUser == null) return null;
            try
            {
                return await discordUser.SendMessageAsync(text, isTTS, embed, options, allowedMentions, components, embeds); ;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<BotUser>> GetUpComingUsersBrithday()
        {
            var users = new List<BotUser>();


            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();

                var TodayMonth = DateTime.UtcNow.Month;
                var TodayDay = DateTime.UtcNow.Day;

                using (var uow = unitOfWorkManager.Begin())
                {
                    IQueryable<BotUser> query = await _repository.GetQueryableAsync();

                    IQueryable<BotUser> usersQuery = query.Where(x => x.Birthday.Month == TodayMonth && x.Birthday.Day == TodayDay);

                    users = await usersQuery.ToListAsync();


                }

            }
            return users;

        }
        public async Task<IQueryable<BotUser>> GetUsersBrithday()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();


                using (var uow = unitOfWorkManager.Begin())
                {
                    var users = await _repository.GetQueryableAsync(a =>
                                 a.Birthday != DateTime.MinValue
                                );

                    return users;
                }

            }

        }

        public async Task<BotUser> addBotUser(string username, string userMail, ulong userId, DateTime birthday, DateTime nextNotifyTime)
        {
            BotUser bUser = new BotUser();
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();


                using (var uow = unitOfWorkManager.Begin())
                {
                     var user = await _repository.FindAsync(x=> x.DiscordUserId == userId);
                    if (user is not null)
                    {
                        user.DiscordUserId = userId;
                        user.Birthday = birthday;
                        user.NextNotifyTIme = nextNotifyTime;
                        await _repository.UpdateAsync(user);
                        await _repository.SaveChangesAsync(true);

                        bUser = user;
                        return bUser;
                    }

                    var botUser = new BotUser().Create(username, userMail, userId, birthday, nextNotifyTime);
                    await _repository.InsertAsync(botUser);
                    await _repository.SaveChangesAsync(true);

                    bUser = botUser;
                }

            }
            return bUser; ;
        }

        public async Task<BotUser> updateBotUser(string username, string userMail, string jiraAccountId, string bitbucketAccountId, ulong userId)
        {
            BotUser bUser = new BotUser();
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();


                using (var uow = unitOfWorkManager.Begin())
                {
                    var user = await _repository.FindAsync(x => x.DiscordUserId == userId);

                    user.Username = username;
                    user.UserMail = userMail;
                    user.JiraAccountID = jiraAccountId;

                    await _repository.UpdateAsync(user);
                    await _repository.SaveChangesAsync(true);

                    bUser = user;
                }

            }
            return bUser;
        }

        public async Task<BotUser> GetUser(ulong Id)
        {
            BotUser bUser = new BotUser();
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();


                using (var uow = unitOfWorkManager.Begin())
                {
                    bUser = await _repository.FindAsync(a => a.DiscordUserId == Id);
                }

            }
            return bUser;
        }
        public async Task<BotUser> GetUserByJiraId(string jiraId)
        {
            BotUser bUser = new BotUser();
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();


                using (var uow = unitOfWorkManager.Begin())
                {
                    bUser = await _repository.FindAsync(a => a.JiraAccountID == jiraId);
                }

            }
            return bUser;
        }

        public async Task<BotUser> GetUserByBitbucketId(string bitbucketId)
        {
            BotUser bUser = new BotUser();
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();


                using (var uow = unitOfWorkManager.Begin())
                {
                    bUser = await _repository.FindAsync(a => a.BitBucketAccountId == bitbucketId);
                }

            }
            return bUser;
        }
    }
}