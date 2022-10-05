using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.BotCore;

namespace BotCore
{
    public class JiraService
    {
        private readonly UsersService usersService;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        public JiraService(IServiceProvider services)
        {
            _services = services;
            this._client = services.GetRequiredService<DiscordSocketClient>();
            this.usersService = services.GetRequiredService<UsersService>();
        }
        public async Task<IMessage> TrySendMessageToUserAsync(string jiraId, string text = null, bool mentionUser = false, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed[] embeds = null)
        {
            var user = await usersService.GetUserByJiraId(jiraId);
            return await usersService.TrySendMessageToUserAsync(user.DiscordUserId, mentionUser ? user.ToUserMention().PadRight(1) : "" + text, isTTS, embed, options, allowedMentions, components);
        }
        public async Task<IUser> GetDisocrdUserByJiraIdAsync(string jiraId)
        {
            var target = await usersService.GetUserByJiraId(jiraId);
            if (target is null)
                return null;

            var userId = target.DiscordUserId;
            return await _client.GetUserAsync(userId);
        }
    }
}
