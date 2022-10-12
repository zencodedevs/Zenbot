using Discord.Interactions;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotCore.Extenstions;
using BotCore.Entities;
using BotCore.Interactions.Preconditions;
using BotCore.Interactions.Authentication.Forms;
using BotCore.Services;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Interactions.Authentication
{

    // Here is the part that New users should enter the server password so the can be part of this server
    [RequireBotPermission(GuildPermission.ManageRoles)]
    public class AuthenticationModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public BotConfiguration botConfiguration { get; set; }
        public EventService EventService { get; set; }
        public GuildService guildService { get; set; }


        [ComponentInteraction("button-admin-setup-authentication-password", true)]
        [RequireGuildRole(RequireGuildRole.RoleType.UnVerified)]
        [RateLimit(10, 1, RateLimit.RateLimitType.User, RateLimit.RateLimitBaseType.BaseOnMessageComponentCustomId)]
        public async Task btnAuthentication()
        {
            await RespondWithModalAsync<AuthenticationForm>("modal-admin-setup-authentication-password");
        }


        // The Modal which unverified user should enter their password
        [ModalInteraction("modal-admin-setup-authentication-password", true)]
        public async Task modalAuthentication(AuthenticationForm modal)
        {
            await DeferAsync(true);

            if (!modal.Password.Equals(Context.BotGuild.AuthenticationPassword))
            {
                var emebd = new EmbedBuilder()
                {
                    Title = "Wrong AuthenticationPassword !",
                    Description = "Your password is not correct! Please contact admin for providing the righ password.",
                    ThumbnailUrl = "https://img.icons8.com/fluency/200/restriction-shield.png",
                    Color = Color.Red
                }.Build();

                await FollowupAsync(embed: emebd, ephemeral: true);
                return;
            }

            var embed = new EmbedBuilder()
            {
                Title = "Be Carefull !",
                Description = "Make sure your direct (DM) is open to send a message then click on the **confirm** button.",
                ThumbnailUrl = "https://img.icons8.com/fluency/344/important-mail.png",
                Color = 506623
            }.Build();

            var component = new ComponentBuilder()
                .WithButton("Confirm", "button-admin-setup-authentication-password-confirm", ButtonStyle.Success, new Emoji("⚒"), null, false, 0);

            await FollowupAsync(embed: embed, components: component.Build(), ephemeral: true);
        }


        /// <summary>
        /// Errro handling if the user's direct is not open, first they should open thier direct so the bot can send them message and 
        /// on boarding file
        /// </summary>
        /// <returns></returns>
        [ComponentInteraction("button-admin-setup-authentication-password-confirm")]
        [RequireGuildRole(RequireGuildRole.RoleType.UnVerified)]
        [RateLimit(10, 1, RateLimit.RateLimitType.User, RateLimit.RateLimitBaseType.BaseOnMessageComponentCustomId)]
        [RequireGuildSetup(RequireGuildSetup.GuildSetupType.LoggerChannel)]
        public async Task confirm()
        {
            await DeferAsync();

            if (!string.IsNullOrEmpty(Context.BotGuild.GreetingFilePath))
            {
                if (!System.IO.File.Exists(Context.BotGuild.GreetingFilePath))
                {
                    await FollowupAsync("File not found. Please contact HR for providing the file", ephemeral: true);
                    return;
                }
                try
                {
                    await Context.User.SendFileAsync(
                        filePath: Context.BotGuild.GreetingFilePath,
                        text: string.Format(Context.BotGuild.GreetingMessage, Context.User.Username));
                }
                catch
                {
                    await FollowupAsync("The bot can't send message in your direct, make sure yoru direct is open.", ephemeral: true);
                    return;
                }
            }

            await (Context.User as IGuildUser).AddRoleAsync(Context.BotGuild.VerifiedRoleId);
            var welcomeMessage = await guildService.GetWelcomeMessageAsync(Context.BotGuild.Id);

            // Message text and replace the {username} with Discord username
            var wMessage = welcomeMessage.Message.Replace("{username}", $"<@{Context.User.Id}>");

            var embed = new EmbedBuilder()
            {
                Title = "Happy Birthday",
                Description = $"{wMessage} \n\n  @everyone  ",
                Color = Color.Purple,
                ThumbnailUrl = "https://img.icons8.com/fluency/200/confetti.png",
                Author = new EmbedAuthorBuilder()
                {
                    Name = Context.User.Username,
                    IconUrl = Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl()
                }
            }.Build();

            await (Context.User as IGuildUser).RemoveRoleAsync(Context.BotGuild.UnVerifiedRoleId);
            var loggerChannel = (GuildChannel)Context.Data;
            await Context._channelService.SendMessageAsync(loggerChannel.ChannelId, Context.User.ToUserMention(), embed: embed);

        }
    }
}

