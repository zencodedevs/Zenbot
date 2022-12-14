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

namespace BotCore.Services.Bitbucket
{
    public class BitbucketService
    {
        private readonly UserService usersService;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        public BitbucketService(IServiceProvider services)
        {
            _services = services;
            _client = services.GetRequiredService<DiscordSocketClient>();
            usersService = services.GetRequiredService<UserService>();
        }
        public async Task<IMessage> SendMessageToUserAsync(string bitbucketId, string text = null, bool mentionUser = false, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageComponent components = null, Embed[] embeds = null)
        {
            var user = await usersService.GetUserByBitbucketId(bitbucketId);
            if(user is not null && user.IsEnableIntegration)
            {
                return await usersService.SendMessageToUserAsync(user.DiscordId, mentionUser ? user.ToUserMention().PadRight(1) : "" + text, isTTS, embed, options, allowedMentions, components);
            }
            return null;
        }
        public async Task<IUser> GetDisocrdUserByBitBucketIdAsync(string bitbucketId)
        {
            var target = await usersService.GetUserByBitbucketId(bitbucketId);
            if (target is null)
                return null;

            var userId = target.DiscordId;
            return await _client.GetUserAsync(userId);
        }
    }
}
