using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotCore.Entities;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace BotCore.Interactions.Shared
{

    public class SharedButtonModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        private const string _btnReturnButtonLabelName = nameof(ReturnButtonLabel);
        public static string ReturnButtonLabelCustomId
        {
            get
            {
                return _btnReturnButtonLabelName + ":" + Guid.NewGuid().ToString();
            }
        }
        [RateLimit(10, baseType: RateLimit.RateLimitBaseType.BaseOnMessageComponentCustomId)]
        [ComponentInteraction(_btnReturnButtonLabelName + ":*")]
        public async Task ReturnButtonLabel(string customId)
        {
            var btn = (Context.Interaction as SocketMessageComponent).Message
                .Components.SelectMany(a => a.Components).FirstOrDefault(a => a.CustomId == $"{_btnReturnButtonLabelName}:" + customId) as ButtonComponent;

            await RespondAsync(btn.Label, ephemeral: true);
        }
        public static ButtonBuilder GetButtonReturnLabelName(string label, ButtonStyle style = ButtonStyle.Primary, IEmote emote = null)
        => new ButtonBuilder(label, ReturnButtonLabelCustomId, style, null, emote, false);



    }
}
