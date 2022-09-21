using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotCore.Interactions;

namespace BotCore
{
    public static class MessageExtenstions
    {
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
