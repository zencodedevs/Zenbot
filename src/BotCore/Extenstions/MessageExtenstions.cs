using BotCore.Addons;
using BotCore.Entities;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Extenstions
{
    public static class MessageExtenstions
    {
        /// <summary>
        /// Provides a timeout for user and then if the user didn't responed we will do next action
        /// </summary>
        /// <returns></returns>
        public static async Task WhenNoResponse(this IUserMessage message, CustomSocketInteractionContext context, TimeSpan timeout, Action<IUserMessage> action)
        {
            var userResponse = await context.ReadUserMessageComponentFromMessageAsync(context.User.Id, message, timeout);
            if (userResponse is null)
            {
                action.Invoke(message);
            }
        }
    }
}
