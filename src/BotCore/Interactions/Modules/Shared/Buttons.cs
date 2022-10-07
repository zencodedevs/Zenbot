using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Zenbot.BotCore.Interactions;

namespace BotCore.Shared
{
    public class Buttons : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public static string BtnTextCustomId
        {
            get
            {
                return "BtnText:" + Guid.NewGuid().ToString();
            }
        }
        [ComponentInteraction("BtnText:*")]
        public async Task btnText(string customId)
        {
            var btn = (Context.Interaction as SocketMessageComponent).Message
                .Components.SelectMany(a => a.Components).FirstOrDefault(a => a.CustomId == "BtnText:" + customId) as ButtonComponent;

            await RespondAsync(btn.Label, ephemeral: true);
        }
    }
}
