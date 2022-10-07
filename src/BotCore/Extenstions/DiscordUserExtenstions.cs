using Discord;
using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Extenstions
{
    public static class DiscordExtenstions
    {

        public static string Mention(this IChannel channel) => MentionUtils.MentionChannel(channel.Id);

        private static string ToDiscordUserId(ulong id)
            => MentionUtils.MentionUser(id);

        public static string ToUserMention(this BotUser user) => ToDiscordUserId(user.DiscordId);

        public static string ToUserMention(this IUser user) => ToDiscordUserId(user.Id);

    }
}
