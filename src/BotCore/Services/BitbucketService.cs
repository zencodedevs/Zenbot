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
    public class BitbucketService
    {
        private readonly UsersService usersService;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        public BitbucketService(IServiceProvider services)
        {
            _services = services;
            this._client = services.GetRequiredService<DiscordSocketClient>();
            this.usersService = services.GetRequiredService<UsersService>();
        }
        public async Task<IMessage> TrySendMessageToUserAsync(string bitbucketId, string text = null, bool mentionUser = false, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed[] embeds = null)
        {
            var user = await usersService.GetUserByBitbucketId(bitbucketId);
            return await usersService.TrySendMessageToUserAsync(user.DiscordUserId, mentionUser ? user.ToUserMention().PadRight(1) : "" + text, isTTS, embed, options, allowedMentions, components);
        }
        public async Task<IUser> GetDisocrdUserByBitBucketIdAsync(string bitbucketId)
        {
            var target = await usersService.GetUserByBitbucketId(bitbucketId);
            if (target is null)
                return null;

            var userId = target.DiscordUserId;
            return await _client.GetUserAsync(userId);
        }
    }
}
