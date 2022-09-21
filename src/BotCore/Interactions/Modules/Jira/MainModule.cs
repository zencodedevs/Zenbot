using Discord;
using Discord.Interactions;
using System;
using System.Threading.Tasks;
using Zenbot.BotCore.Interactions.Forms;
using Zenbot.BotCore.Models;

namespace Zenbot.BotCore.Interactions.Modules.Jira
{
    [Group("jira", "jira info for sending message whenever an event occurs!")]
    public class MainModule : InteractionModuleBase<SocketInteractionContext>
    {
        public UsersService UsersService { get; set; }


        [SlashCommand("info", "Getting the information from discord user")]
        public async Task login()
        {
            await RespondWithModalAsync<JiraLoginForm>("jira-info-modal");
        }

        [ModalInteraction("jira-info-modal", true)]
        public async Task login_modal(JiraLoginForm form)
        {
            await DeferAsync(true);

            var id = Context.User.Id;

            Embed embed = new EmbedBuilder()
            {
                Title = "Your Information Updated",
                Description = String.Format(
                "Your Discord Id: {0}\n" +
                "Your Id: {1}\n" +
                "Your Email: {2}\n" +
                "Your Username: {3}\n", Context.User.Id, form.Id, form.Email, form.Username),
                ThumbnailUrl = Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl(),
                Color = Color.Green
            }.Build();

            await UsersService.updateBotUser(form.Username, form.Email, form.Id, id);

            await FollowupAsync(embed: embed, ephemeral: true);
        }
    }
}
