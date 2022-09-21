using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace BotCore
{
    public class JiraLoginForm : IModal
    {
        public string Title => "Login";

        [InputLabel("Id")]
        [ModalTextInput("Id", TextInputStyle.Short, "Enter your jira id", 1, 200, null)]
        [RequiredInput(true)]
        public string Id { get; set; }

        [InputLabel("Username")]
        [ModalTextInput("username", TextInputStyle.Short, "Enter your jira username", 1, 200, null)]
        [RequiredInput(true)]
        public string Username { get; set; }

        [InputLabel("Email Address")]
        [RequiredInput(true)]
        [ModalTextInput("email", TextInputStyle.Short, "Enter your jira email", 1, 200, null)]
        public string Email { get; set; }



    }
}
