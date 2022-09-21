using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.BotCore.Interactions
{
    public interface ICustomInteractionContext : IInteractionContext
    {
        //public SocketInteraction InteractionOverrided { get; }
        //public SocketInteraction SetOverridedInteraction(SocketInteraction interaction);
    }

    public class SampleInteractionContext : InteractionContext, ICustomInteractionContext
    {
        //private SocketInteraction _overridedInteraction;

        public SampleInteractionContext(IDiscordClient client, IDiscordInteraction interaction, IMessageChannel channel = null) : base(client, interaction, channel)
        {

        }

        //public SocketInteraction InteractionOverrided => _overridedInteraction;
        //public SocketInteraction SetOverridedInteraction(SocketInteraction interaction)
        //{
        //    this._overridedInteraction = interaction;
        //    return InteractionOverrided;
        //}
    }
}
