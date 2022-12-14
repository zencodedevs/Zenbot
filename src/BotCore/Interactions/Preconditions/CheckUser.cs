using BotCore.Entities;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Interactions.Preconditions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CheckUser : PreconditionAttribute
    {

        public CheckUserType checkType;
        public CheckUser(CheckUserType checkType)
        {
            this.checkType = checkType;
        }
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {

            if (context.Interaction is not SocketMessageComponent component)
                return Task.FromResult(PreconditionResult.FromError("CheckUser is only supported for message components."));

            if (checkType == CheckUserType.MessageMention)
            {
                if ((context.Interaction as SocketMessageComponent).Message.MentionedUsers.Select(a => a.Id).Contains(context.User.Id))
                    return Task.FromResult(PreconditionResult.FromSuccess());
                else
                    return Task.FromResult(PreconditionResult.FromError("Access Denied."));
            }

            var context_ = context as CustomSocketInteractionContext;
            var matchs = context_.SegmentMatches;
            var param = matchs.FirstOrDefault();

            if (ulong.TryParse(param.Value ?? "0", out ulong id))
            {
                if (context.User.Id != id)
                {
                    return Task.FromResult(PreconditionResult.FromError("Context user cannot operate this component."));
                }
                else
                    return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
                return Task.FromResult(PreconditionResult.FromError("Parse cannot be done if no user ID exists."));

        }

        public enum CheckUserType
        {
            MessageMention,
            CustomId
        }
    }



}
