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
using Zenbot.Domain.Shared.Common;

namespace BotCore.Interactions.Authentication
{

    // Here is the part that New users should enter the server password so the can be part of this server
    [RequireBotPermission(GuildPermission.ManageRoles)]
    public class AuthenticationModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public BotConfiguration botConfiguration { get; set; }
        public EventService EventService { get; set; }
        public GuildService guildService { get; set; }
        public ChannelService _channelService { get; set; }
        public WelcomeMessageService welcomeMessageService { get; set; }
        public BoardingServices boardingServices { get; set; }
        public BoardingFileService boardingFileService { get; set; }


        [ComponentInteraction("button-admin-setup-authentication-password", true)]
        [RequireGuildRole(RequireGuildRole.RoleType.UnVerified)]
        [RateLimit(10, 1, RateLimit.RateLimitType.User, RateLimit.RateLimitBaseType.BaseOnMessageComponentCustomId)]
        public async Task btnAuthentication()
        {
            await RespondWithModalAsync<AuthenticationForm>("modal-admin-setup-authentication-password");

            // Log the message
            var message = $"Try to sign in";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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

            var welcomeMessage = await guildService.GetWelcomeMessageAsync(Context.BotGuild.Id);

            // check if welcome message is enable or not
            if (await welcomeMessageService.CheckIfWelcomeMessageIsEnable(Context.BotGuild.Id))
            {
                // Message text and replace the {username} with Discord username
                var wMessage = StaticData.BoardingDefaultMessage.Replace("{username}", $"<@{Context.User.Id}>");

                if (welcomeMessage != null)
                {
                    wMessage = welcomeMessage.Message.Replace("{username}", $"<@{Context.User.Id}>");
                }


                // Boarding Message settings
                var brMessage = StaticData.BoardingDefaultMessage.Replace("{username}", $"<@{Context.User.Id}>");

                var boardingMessage = await boardingServices.CheckIfBoardingMessageIsEnable(Context.BotGuild.Id);

                //Send boarding message in DM
                if (boardingMessage != null)
                {
                    brMessage = boardingMessage.Message.Replace("{username}", $"<@{Context.User.Id}>");

                    try
                    {
                        await Context.User.SendFileAsync(
                            filePath: "",
                            text: brMessage);

                        // Check for attache files for this boarding message
                        var brFiles = await boardingFileService.CheckIfBoardingFilesExist(boardingMessage.Id);
                        if (brFiles != null)
                        {
                            foreach (var item in brFiles)
                            {
                                await Context.User.SendFileAsync(
                               filePath: item.FilePath,
                               text: "Attached file");
                            }
                        }

                    }
                    catch
                    {
                        await FollowupAsync("The bot can't send message in your direct, make sure yoru direct is open.", ephemeral: true);
                        return;
                    }
                }


                await (Context.User as IGuildUser).AddRoleAsync(Context.BotGuild.VerifiedRoleId);

                var embed = new EmbedBuilder()
                {
                    Title = $"{Context.User.Username} joined",
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

                // check if welcome message is Enable
                if (await welcomeMessageService.CheckIfWelcomeMessageIsEnable(Context.BotGuild.Id))
                {
                    await Context._channelService.SendMessageAsync(loggerChannel.ChannelId, Context.User.ToUserMention(), embed: embed);
                }

            }

            // Log the message
            var message = $"User joined the server successfully";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }
    }
}

