using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.WelcomeMessageFormModal
{
    public class WelcomeMessageForm : IModal
    {
        public string Title => "Welcome Message";

        [InputLabel("Message")]
        [ModalTextInput("message", Discord.TextInputStyle.Paragraph, "Welcome Message, use {username} where ever you want to put the Discord's username", 1, 500, null)]
        [RequiredInput]
        public string Message { get; set; }

    }
}
