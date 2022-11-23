using BotCore.Extenstions;
using BotCore.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;

namespace BotCore.Services.Jira
{
    public class JiraService
    {
        private readonly UserService usersService;
        private readonly BotUserGuildServices botUserGuildServices;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        public JiraService(IServiceProvider services)
        {
            _services = services;
            _client = services.GetRequiredService<DiscordSocketClient>();
            usersService = services.GetRequiredService<UserService>();
            botUserGuildServices = services.GetRequiredService<BotUserGuildServices>();
        }
        public async Task<IMessage> SendMessageToUserAsync(string jiraId, string text = null, bool mentionUser = false, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed[] embeds = null)
        {
            var user = await usersService.GetUserByJiraId(jiraId);
            if(user is not null && user.IsEnableIntegration)
            {
              return await usersService.SendMessageToUserAsync(user.DiscordId, mentionUser ? user.ToUserMention().PadRight(1) : "" + text, isTTS, embed, options, allowedMentions, components);
               
            }
            return null;
        }
        public async Task<IUser> GetDisocrdUserByJiraIdAsync(string jiraId)
        {
            var target = await usersService.GetUserByJiraId(jiraId);
            if (target is null)
                return null;

            var userId = target.DiscordId;
            return await _client.GetUserAsync(userId);
        }

        public async Task<UserInfoDto> GetUserInfo(string jiraID)
        {
            var user = await usersService.GetUserByJiraId(jiraID);
            var userinfo =await botUserGuildServices.GetUserInfoAsync(user.Id);
            var info = new UserInfoDto
            {
                UserId = userinfo.BotUser.DiscordId,
                Username = userinfo.BotUser.Username,
                GuildId = userinfo.Guild.GuildId,
                GuildName = userinfo.Guild.GuildName
            };
            return info;
        }
    }
}
