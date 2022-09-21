using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using EllipticCurve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.BotCore.Interactions.SlashCommands
{
    [Group("jira", "jira test")]
    public class MainModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public UsersService UsersService { get; set; }

        [SlashCommand("account", "account setting")]
        public async Task account()
        {
            await DeferAsync();

            var component = new ComponentBuilder()
                  .WithButton("Yes", $"button-jira-account-{nameof(confirm)}:{Context.User.Id}", ButtonStyle.Success, null, null, false, 0)
                  .WithButton("No", $"button-jira-account-{nameof(cancel)}:{Context.User.Id}", ButtonStyle.Danger, null, null, false, 0)
                  .Build();

            var timeout = TimeSpan.FromSeconds(50);

            var msg = await FollowupAsync(
                "Are you sure to fill your information ?" +
                $"\n**This closes in {timeout.ToUtcDiscordUnixTime()}**",
                components: component);

            await msg.WhenNoResponse(this.Context, timeout, x =>
            {
                x.DeleteAsync();
            });
        }

        [ComponentInteraction("button-jira-account-cancel:*", true)]
        [CheckUser(CheckUser.CheckUserType.CustomId)]
        public async Task cancel(ulong id)
        {
            await DeferAsync();

            await (Context.Interaction as SocketMessageComponent).Message.ModifyAsync(x =>
            {
                x.Content = $"You canceled the task.";
                x.Components = new ComponentBuilder().Build();
            });
        }
        [ComponentInteraction("button-jira-account-confirm:*", true)]
        [CheckUser(CheckUser.CheckUserType.CustomId)]
        public async Task confirm(ulong userId)
        {
            await DeferAsync();

            var component = new ComponentBuilder()
                  .WithButton("Fill Info Here", $"button-jira-account-{nameof(enterInfo)}:{Context.User.Id}", ButtonStyle.Primary, null, null, false, 0)
                  .WithButton("Fill Info ont the Web", null, ButtonStyle.Link, null, "https://localhost:5001/api", false, 0);

            var msg = (Context.Interaction as SocketMessageComponent).Message;

            var timeout = TimeSpan.FromSeconds(10);

            await msg.WhenNoResponse(this.Context, timeout, async (x) =>
            {
                await x.ModifyAsync(a =>
                {
                    a.Content = $"Welcome, you have two ways to enter your account info." +
                     $"\n**This closes in {timeout.ToUtcDiscordUnixTime()}**";

                    a.Components = component.Build();
                });
            });
        }

        [ComponentInteraction("button-jira-account-enterInfo:*", true)]
        [CheckUser(CheckUser.CheckUserType.CustomId)]

        public async Task enterInfo(ulong id)
        {
            await RespondWithModalAsync<JiraLoginForm>("jira-login-modal");
            await (Context.Interaction as SocketMessageComponent).Message.DeleteAsync();
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
