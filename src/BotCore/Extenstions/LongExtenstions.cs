using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore
{
    public static class LongExtenstions
    {
        public static string ToUserMention(this ulong id)
        {
            return $"<@{id}>";
        }
    }
}
