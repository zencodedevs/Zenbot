using Discord;
using Discord.Interactions;
using EllipticCurve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Interactions.SlashCommands
{
    [Group("jira", "jira test")]
    public class MainModule : InteractionModuleBase<SocketInteractionContext>
    {
        public UsersService UsersService { get; set; }


        [SlashCommand("login", "test")]
        public async Task login()
        {
            await RespondWithModalAsync<JiraLoginForm>("jira-login-modal");
        }

        [ModalInteraction("jira-login-modal", true)]
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
