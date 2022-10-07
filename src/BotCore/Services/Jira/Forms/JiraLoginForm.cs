using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Interactions;

namespace BotCore.Services.Jira.Forms
{
    // Modal form for adding jira info
    public class JiraLoginForm : IModal
    {
        public string Title => "Login";

        [InputLabel("Jira Account JiraId")]
        [ModalTextInput("JiraId", TextInputStyle.Short, "Enter your jira account id", 1, 200, null)]
        [RequiredInput(true)]
        public string JiraId { get; set; }

        [InputLabel("Bitbucket Account JiraId")]
        [ModalTextInput("bitbucketId", TextInputStyle.Short, "Enter your bitbucket account id", 1, 200, null)]
        [RequiredInput(true)]
        public string bitbucketId { get; set; }

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
