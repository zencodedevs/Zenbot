using BotCore.Entities;
using BotCore.Extenstions;
using BotCore.Interactions.Preconditions;
using BotCore.Services;
using BotCore.Services.Jira.Forms;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using EllipticCurve;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Services.Jira.Modules
{
    [Group("external", "We can add other external account like Jira and bitbucket account JiraId here")]
    public class MainModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public UserService UsersService { get; set; }
        public ChannelService _channelService { get; set; }

        // Command for getting the Jira info from user
        [SlashCommand("account", "External integrated account")]
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
                  .WithButton("Yes, i am ready", $"button-external-account-confirm:{Context.User.Id}", ButtonStyle.Success, new Emoji("✔"), null, false, 0)
                  .WithButton("No, later", $"button-external-account-cancel:{Context.User.Id}", ButtonStyle.Danger, new Emoji("❌"), null, false, 0)
                  .Build();

            var msg = await FollowupAsync(Context.User.ToUserMention(), embed: embed, components: component);

            await msg.WhenNoResponse(Context, timeout, x =>
            {
                x.DeleteAsync();
            });


        }

        // Cancel the operation
        [ComponentInteraction("button-external-account-cancel:*", true)]
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
        [ComponentInteraction("button-external-account-confirm:*", true)]
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
                  .WithButton("Fill Info Here", $"button-external-account-{nameof(enterInfo)}:{Context.User.Id}", ButtonStyle.Primary, null, null, false, 0)
                  .WithButton("Fill Info ont the Web", null, ButtonStyle.Link, null, "https://localhost:5001/api", false, 0);

            var msg = (Context.Interaction as SocketMessageComponent).Message;

            await msg.ModifyAsync(a =>
            {
                a.Embed = embed;
                a.Content = Context.User.ToUserMention();
                a.Components = component.Build();
            });

            await msg.WhenNoResponse(Context, timeout, async (x) =>
            {
                await x.DeleteAsync();
            });
        }



        [ComponentInteraction("button-external-account-enterInfo:*", true)]
        [CheckUser(CheckUser.CheckUserType.CustomId)]

        public async Task enterInfo(ulong id)
        {
            await RespondWithModalAsync<ExternalAccountForm>("external-account-modal");
            await (Context.Interaction as SocketMessageComponent).Message.DeleteAsync();
        }


        // Modal Interaction which is geeting Modal data
        [ModalInteraction("external-account-modal", true)]
        public async Task external_modal(ExternalAccountForm form)
        {
            await DeferAsync(true);

            Embed embed = new EmbedBuilder()
            {
                Title = "Your Information Updated",
                Description =
                $"Your Jira Account ID: `{form.JiraId}`\n" +
                $"Your Bitbuckt Account ID: `{form.bitbucketId}`\n" +
                $"Your Discord JiraId: `{Context.User.Id}`\n",
                ThumbnailUrl = Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl(),
                Color = Color.Green
            }.Build();

            await UsersService.UpdateAsync(Context.BotUser.Id, x =>
            {
                x.BitBucketAccountId = form.bitbucketId;
                x.JiraAccountID = form.JiraId;
                x.GuildId = Context.BotGuild.Id;
            });

            await FollowupAsync(embed: embed, ephemeral: true);


            // Log the message
            var message = $"Created external accounts `Jira, BitBucket` ";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);
        }
    }
}
