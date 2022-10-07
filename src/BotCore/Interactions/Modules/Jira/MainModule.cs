using BotCore;
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
    [Group("external", "We can add other external account like Jira and bitbucket account Id here")]
    public class MainModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public UsersService UsersService { get; set; }


        // Command for getting the Jira info from user
        [SlashCommand("account", "account setting")]
        public async Task account()
        {
            await DeferAsync();

            var timeout = TimeSpan.FromSeconds(50);
            var embed = new EmbedBuilder()
            {
                Title = "Are you ready ?",
                Description =
                $"**Are you ready to complete your information ?**\n" +
                $"Timeout: {timeout.ToUtcDiscordUnixTime()}",
                ThumbnailUrl = "https://img.icons8.com/fluency/480/check-all.png",
                Color = 6542957,
            }.Build();

            var component = new ComponentBuilder()
                  .WithButton("Yes, i am ready", $"button-jira-account-confirm:{Context.User.Id}", ButtonStyle.Success, new Emoji("✔"), null, false, 0)
                  .WithButton("No, later", $"button-jira-account-cancel:{Context.User.Id}", ButtonStyle.Danger, new Emoji("❌"), null, false, 0)
                  .Build();

            var msg = await FollowupAsync(Context.User.ToUserMention(), embed: embed, components: component);

            await msg.WhenNoResponse(this.Context, timeout, x =>
            {
                x.DeleteAsync();
            });
        }

        // Cancel the operation
        [ComponentInteraction("button-jira-account-cancel:*", true)]
        [CheckUser(CheckUser.CheckUserType.CustomId)]
        public async Task cancel(ulong id)
        {
            await DeferAsync();
            await (Context.Interaction as SocketMessageComponent).Message.ModifyAsync(x =>
            {
                var embed = new EmbedBuilder()
                {
                    Title = "Task Canceled",
                    Description = "You canceled the task, but you can run new one with `/external account`.",
                    ThumbnailUrl = "https://img.icons8.com/fluency/480/delete-sign.png",
                    Color = 14946816,
                }.Build();

                x.Content = $"<@{id}>";
                x.Embed = embed;
                x.Components = new ComponentBuilder().Build();
            });
        }

// Confirm the operation
        [ComponentInteraction("button-jira-account-confirm:*", true)]
        [CheckUser(CheckUser.CheckUserType.CustomId)]
        public async Task confirm(ulong id)
        {
            await DeferAsync();

            var timeout = TimeSpan.FromSeconds(60);
            var embed = new EmbedBuilder()
            {
                Title = "Hold On !",
                Description =
                "**Are you sure to fill your information ?**\n" +
                "🤖 ` 1.` **The bot**, click on the  **Fill out here.**\n" +
                "🌐 ` 2.` **The web,** fill on the site, click on the  **Fill out on the web**.\n\n" +
               $"⌛ Timeout: {timeout.ToUtcDiscordUnixTime()}",
                ThumbnailUrl = "https://img.icons8.com/fluency/200/verified-account.png",
                Color = 1364764
            }.Build();

            var component = new ComponentBuilder()
                  .WithButton("Fill Info Here", $"button-jira-account-{nameof(enterInfo)}:{Context.User.Id}", ButtonStyle.Primary, null, null, false, 0)
                  .WithButton("Fill Info ont the Web", null, ButtonStyle.Link, null, "https://localhost:5001/api", false, 0);

            var msg = (Context.Interaction as SocketMessageComponent).Message;
            
            await msg.ModifyAsync(a =>
            {
                a.Embed = embed;
                a.Content = Context.User.ToUserMention();
                a.Components = component.Build();
            });

            await msg.WhenNoResponse(this.Context, timeout, async (x) =>
            {
                await x.DeleteAsync();
            });
        }



        [ComponentInteraction("button-jira-account-enterInfo:*", true)]
        [CheckUser(CheckUser.CheckUserType.CustomId)]

        public async Task enterInfo(ulong id)
        {
            await RespondWithModalAsync<JiraLoginForm>("jira-login-modal");
            await (Context.Interaction as SocketMessageComponent).Message.DeleteAsync();
        }


        // Modal Interaction which is geeting Modal data
        [ModalInteraction("jira-login-modal", true)]
        public async Task login_modal(JiraLoginForm form)
        {
            await DeferAsync(true);

            var id = Context.User.Id;

            Embed embed = new EmbedBuilder()
            {
                Title = "Your Information Updated",
                Description =
                $"Your Jira Account ID: `{form.Id}`\n" +
                $"Your Bitbuckt Account ID: `{form.bitbucketId}`\n" +
                $"Your Email: `{form.Email}`\n" +
                $"Your Username: `{form.Username}`\n" +
                $"Your Discord Id: `{Context.User.Id}`\n",
                ThumbnailUrl = Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl(),
                Color = Color.Green
            }.Build();

            await UsersService.updateBotUser(form.Username, form.Email, form.Id, form.bitbucketId, id);

            await FollowupAsync(embed: embed, ephemeral: true);
        }
    }
}
