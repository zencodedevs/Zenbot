using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BotCore.Extenstions
{
    public static class DatetimeExtenstions
    {
        /// <summary>
        ///  For time out the messages, whenever we want user to response within specific time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>


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
