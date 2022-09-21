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

    }

    public class CustomSocketInteractionContext : InteractionContext, ICustomInteractionContext
    {

        public CustomSocketInteractionContext(IDiscordClient client, IDiscordInteraction interaction, IMessageChannel channel = null) : base(client, interaction, channel)
        {

        }

    }
}
