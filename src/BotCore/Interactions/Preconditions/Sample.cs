namespace Zenbot.BotCore.Interactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Interactions;
    public class Sample : PreconditionAttribute
    {
        public new string ErrorMessage = $"Sample Precondition.";
        public Sample() { }
        public override Task<PreconditionResult> CheckRequirementsAsync(IInteractionContext context, ICommandInfo commandInfo, IServiceProvider services)
        {
            if (true)
                return Task.FromResult(PreconditionResult.FromSuccess());
            else
                return Task.FromResult(PreconditionResult.FromError(this.ErrorMessage));
        }
    }
}
