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
    public class ExternalAccountForm : IModal
    {
        public string Title => "External accounts for intergration";

        [InputLabel("Jira Account ID")]
        [ModalTextInput("JiraId", TextInputStyle.Short, "Enter your jira account id", 1, 200, null)]
        [RequiredInput(true)]
        public string JiraId { get; set; }

        [InputLabel("Bitbucket Account ID")]
        [ModalTextInput("bitbucketId", TextInputStyle.Short, "Enter your bitbucket account id", 1, 200, null)]
        [RequiredInput(true)]
        public string bitbucketId { get; set; }

       
    }
}
