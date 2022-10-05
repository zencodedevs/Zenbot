using Discord;
using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore
{
    public static class DiscordUserExtenstions
    {
        private static string ToDiscordUserId(ulong id) 
            => MentionUtils.MentionUser(id);

        public static string ToUserMention(this BotUser user) => ToDiscordUserId(user.DiscordUserId);

        public static string ToUserMention(this IUser user) => ToDiscordUserId(user.Id);

    }
}
