using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace BotCore.Interactions.Authentication.Forms
{
    // Modal form for Athentication the users
    public class AuthenticationForm : IModal
    {
        public string Title => "Authentication";

        [InputLabel("AuthenticationPassword")]
        [ModalTextInput("password", TextInputStyle.Short, "Enter your password.", 1, 4000, null)]
        [RequiredInput(true)]
        public string Password { get; set; }


    }
}
