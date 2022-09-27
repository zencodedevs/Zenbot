using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotCore;
namespace Zenbot.BotCore.Interactions
{

    public interface ICustomInteractionContext : IInteractionContext
    {

    }
    // Bot's custome SocketInteractionContext for more costomization
    public class CustomSocketInteractionContext : InteractionContext, ICustomInteractionContext
    {

        public CustomSocketInteractionContext(IDiscordClient client, IDiscordInteraction interaction, IMessageChannel channel = null) : base(client, interaction, channel)
        {

        }

    }
}
