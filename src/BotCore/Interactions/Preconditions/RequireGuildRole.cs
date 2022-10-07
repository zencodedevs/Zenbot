namespace BotCore.Interactions.Preconditions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BotCore.Entities;
    using Discord;
    using Discord.Interactions;
    using Microsoft.Extensions.DependencyInjection;

    public class RequireGuildRole : PreconditionAttribute
    {
        private readonly RoleType _role;
        public RequireGuildRole(RoleType role)
        {
            _role = role;
        }
        public enum RoleType
        {
            Verified,
            UnVerified,
            HR
        }
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context_, ICommandInfo commandInfo, IServiceProvider services)
        {
            IGuildUser guildUser = context_.User as IGuildUser;
            if (guildUser == null)
            {
                return Task.FromResult(PreconditionResult.FromError("Command must be used in a guild channel."));
            }
            var context = context_ as CustomSocketInteractionContext;

            ulong roleId = _role switch
            {
                RoleType.Verified => context.BotGuild.VerifiedRoleId,
                RoleType.UnVerified => context.BotGuild.UnVerifiedRoleId,
                RoleType.HR => context.BotGuild.HrRoleId,
                _ => 0
            };

            if (guildUser.RoleIds.Contains(roleId))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError(ErrorMessage ?? "You don't have permission to run this command!"));
            //return Task.FromResult(PreconditionResult.FromError(ErrorMessage ?? "You need guild role **" + context.Guild.GetRole(roleId).Name + "**."));

        }
    }
}
