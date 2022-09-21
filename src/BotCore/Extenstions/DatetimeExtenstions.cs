using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zenbot
{
    public static class DatetimeExtenstions
    {
        public static string ToUtcDiscordUnixTime(this TimeSpan time)
        {
            return (DateTime.UtcNow + time).ToUnixTimeSeconds().ToDiscordUnixTime();
        }
        public static string ToDiscordUnixTime(this DateTime time)
        {
            return time.ToUnixTimeSeconds().ToDiscordUnixTime();
        }
        
        public static long ToUnixTimeSeconds(this DateTime time)
        {
            return ((DateTimeOffset)time).ToUnixTimeSeconds();
        }

        public static string ToDiscordUnixTime(this long time)
        {
            return $"<t:{time}:R>";
        }
    }
}
