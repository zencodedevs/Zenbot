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
        private static string ToUserId(ulong id) 
            => MentionUtils.MentionUser(id);

        public static string ToUserMention(this BotUser user) => ToUserId(user.UserId);

        public static string ToUserMention(this IUser user) => ToUserId(user.Id);

    }
}
