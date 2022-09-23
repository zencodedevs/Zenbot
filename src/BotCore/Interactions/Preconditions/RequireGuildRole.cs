namespace Discord.Interactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Interactions;
    using Microsoft.Extensions.DependencyInjection;
    using Zenbot.BotCore;

    public class RequireGuildRole : PreconditionAttribute
    {
        private readonly RoleType _role;
        public RequireGuildRole(RoleType role)
        {
            this._role = role;
        }
        public enum RoleType
        {
            Verified,
            UnVerified,
            HR
        }
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {
            IGuildUser guildUser = context.User as IGuildUser;
            if (guildUser == null)
            {
                return Task.FromResult(PreconditionResult.FromError("Command must be used in a guild channel."));
            }

            var config = services.GetRequiredService<BotConfiguration>();

            ulong _roleId = _role switch
            {
                RoleType.Verified => config.Roles.VarifiedId,
                RoleType.UnVerified => config.Roles.UnVarifiedId,
                RoleType.HR => config.Roles.HR,
                _ => 0
            };

            if (guildUser.RoleIds.Contains(_roleId))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }

            return Task.FromResult(PreconditionResult.FromError(ErrorMessage ?? ("You need guild role **" + context.Guild.GetRole(_roleId).Name + "**.")));

        }
    }
}
