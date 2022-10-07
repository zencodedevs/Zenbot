﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotCore.Entities;
using Discord;
using Discord.Interactions;
using SendGrid.Helpers.Mail;
using Twilio.Rest.Trunking.V1;
using Zenbot.Domain.Shared.Entities.Bot;

namespace Discord.Interactions
{
    public class RequireGuildSetup : PreconditionAttribute
    {
        public enum GuildSetupType { RoleId, LoggerChannel }

        private readonly GuildSetupType setupType;
        public RequireGuildSetup(GuildSetupType guildSetupType)
        {
            setupType = guildSetupType;
        }

        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context_, ICommandInfo commandInfo, IServiceProvider services)
        {
            var context = context_ as CustomSocketInteractionContext;
            switch (setupType)
            {
                case GuildSetupType.RoleId:
                    if (context.BotGuild.VerifiedRoleId == 0 && context.BotGuild.VerifiedRoleId == 0)
                        return Task.FromResult(PreconditionResult.FromError("Verified Role & Unverified Role not found, setup command `/setup roles`."));
                    break;

                case GuildSetupType.LoggerChannel:
                    var channel = context._guildService.GetChannelAsync(context.BotGuild.Id, GuildChannelType.Logger);
                    context.Data = channel;
                    if (channel is null)
                        return Task.FromResult(PreconditionResult.FromError("Verified Role & Unverified Role not found, setup command `/setup roles`."));
                    break;
            }
            return Task.FromResult(PreconditionResult.FromSuccess());
        }
    }
}
