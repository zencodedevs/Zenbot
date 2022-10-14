using BotCore.Entities;
using BotCore.Extenstions;
using BotCore.Services;
using BotCore.Services.Birthday.Forms;
using BotCore.WelcomeMessageFormModal;
using Discord;
using Discord.Interactions;
using Microsoft.CodeAnalysis.Operations;
using NJsonSchema.Validation;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Interactions.Modules.Admin
{
    [Group("setup", "setup guild settings")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]

    [RequireBotPermission(GuildPermission.ManageRoles)]
    [RequireContext(ContextType.Guild)]
    [EnabledInDm(false)]
    public class SetupModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public ChannelService _channelService { get; set; }
        public BirthdayMessageService _birthdayMessageService { get; set; }
        public WelcomeMessageService _welcomeMessageService { get; set; }
        public UserService _userService { get; set; }




        // Setup the Supervisor for your this Guild
        // we can have multiple Supervisor

        [SlashCommand("choose-supervisor", "you can make a user as supervisor")]
        public async Task supervisor()
        {
            await DeferAsync();
            var users = await _userService.GetUsersOfGuild(Context.BotGuild.Id);

            var menu = new SelectMenuBuilder() 
            {
                CustomId = "userMenu",
                Placeholder = "Select a user"
            };
            foreach (var item in users)
            {
                menu.AddOption(item.Username, (item.DiscordId).ToString());
            }

           

            var builder = new ComponentBuilder()
                .WithSelectMenu(menu);

            await ReplyAsync("Whos really lying?", components: builder.Build());
            //await Context._guildService.UpdateAsync(Context.BotGuild.Id, x =>
            //{
            //    x.BotPrefix = prefix;
            //});
            await FollowupAsync($"Channel prefix updated to *");
        }



        // Replace the default bot prefix with your own profex
        [SlashCommand("prefix", "setup server prefix")]
        public async Task prefix([MaxLength(20)] string prefix)
        {
            await DeferAsync();
            await Context._guildService.UpdateAsync(Context.BotGuild.Id, x =>
            {
                x.BotPrefix = prefix;
            });
            await FollowupAsync($"Channel prefix updated to **{prefix}**");
        }


        // Create Or update the Server Guild password for Authentication users
        [SlashCommand("password", "setup server password")]
        public async Task password(string password)
        {
            await DeferAsync();
            await Context._guildService.UpdateAsync(Context.BotGuild.Id, x =>
            {
                x.AuthenticationPassword = password;
            });
            await FollowupAsync($"The password changed to {password} succesfuly.");
        }


        // Select or change the Guild logger channel
        [SlashCommand("logger-channel", "setup logger channel")]
        public async Task logger_channel(ITextChannel channel)
        {
            await DeferAsync();

            var loggerChannel = await _channelService.GetAsync(a => a.Type == GuildChannelType.Logger && a.GuildId == Context.BotGuild.Id);
            if (loggerChannel is not null)
            {
                await _channelService.UpdateAsync(loggerChannel, x => x.Type = GuildChannelType.None);
            }

            var targetChannel = await _channelService.GetOrAddAsync(channel.Id, Context.BotGuild.Id);
            await _channelService.UpdateAsync(targetChannel, x => x.Type = GuildChannelType.Logger);

            await FollowupAsync($"The logger channel changed to **<#{channel.Mention()}>**");
        }


        // Select or change the Guild Birthday Message
        [SlashCommand("birthday-message", "setup birthday message for this guild")]
        public async Task birthday_message()
        {
            await RespondWithModalAsync<BirthdayMessageForm>($"set-brithday-message");
        }

        [ModalInteraction("set-brithday-message", true)]
        public async Task set_modal(BirthdayMessageForm form)
        {
            await DeferAsync();

            await _birthdayMessageService.GetOrAddAsync(true, form.Message, Context.BotGuild.Id);
            await FollowupAsync($"You've adde new message for birthday \n **<#{form.Message}>**");
        }



        // Select or change the Guild Welcome Message
        [SlashCommand("welcome-message", "setup welcome message for this guild")]
        public async Task welcome_message()
        {
            await RespondWithModalAsync<WelcomeMessageForm>($"set-welcome-message");
        }

        [ModalInteraction("set-welcome-message", true)]
        public async Task welcome_modal(WelcomeMessageForm form)
        {
            await DeferAsync();

            await _welcomeMessageService.GetOrAddAsync(true, form.Message, Context.BotGuild.Id);
            await FollowupAsync($"You've adde new welcome Message \n **<#{form.Message}>**");
        }



        // Insert or change the Roles Id for this Guild
        [SlashCommand("roles", "setup server roles")]
        public async Task roles(IRole verified, IRole unVerified, IRole hr)
        {
            await DeferAsync();
            if (new ulong[] { verified.Id, unVerified.Id, hr.Id }.Distinct().Count() != 3)
            {
                await FollowupAsync("duplicated roles are not allowed");
                return;
            }
            await Context._guildService.UpdateAsync(Context.BotGuild, x =>
            {
                x.VerifiedRoleId = verified.Id;
                x.UnVerifiedRoleId = unVerified.Id;
                x.HrRoleId = hr.Id;
            });
            await FollowupAsync($"The server roles updated.");
        }

        // Set the greeting message for this Guild
        [SlashCommand("on-boarding-file", "setup server's greeting message.")]
        public async Task greeting_message(IAttachment grettingFile = null)
        {
            await DeferAsync();

            if (File.Exists(Context.BotGuild.GreetingFilePath))
                File.Delete(Context.BotGuild.GreetingFilePath);

            if (grettingFile is null)
            {
                await Context._guildService.UpdateAsync(Context.BotGuild, x =>
                {
                    x.GreetingFilePath = "";
                });
                return;
            }

            // Download the file from Discord and put in a file in discord
            // then we can just sent this file from discord

            using (WebClient client = new WebClient())
            {
                var directoryPath = $@"guilds/{Context.Guild.Id}/";
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var filePath = directoryPath + grettingFile.Filename;
                if (File.Exists(filePath))
                    File.Delete(filePath);

                client.DownloadFileCompleted += Client_DownloadFileCompleted;
                async void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                {
                    if (e.Error is not null)
                    {
                        await FollowupAsync("can not download the file, try again.");
                        return;
                    }
                    await Context._guildService.UpdateAsync(Context.BotGuild, x =>
                    {
                        x.GreetingFilePath = filePath;
                    });
                    await FollowupAsync("file downloaded, gretting-message updated.");
                }

                client.DownloadFileAsync(new Uri(grettingFile.Url), filePath);
                await FollowupAsync("please wait, we are downloading the file.");
            }

        }


        //Help Command for the whole bot setup
        [SlashCommand("help", "help")]
        public async Task help()
        {
            var embed = new EmbedBuilder()
            .WithTitle("Setup Help")
            .WithDescription(
                $"`Note:` in order to enable user authentication first you should make three Roles in your " +
                $"server `Verified, Unverified, HR` Roles. Then make a channel which only `unverfied` role can see " +
                $"and the whole other channel should be visible only for `Verified` role. \n\n" +
                $"If done, great let's follow these folowing steps: \n\n" +
                $"1. `/setup roles`  Setup your roles for this server so bot can perform it's tasks.\n" +
                $"2. `/setup password` The password which server user will be authenticated.\n" +
                $"3. `/setup logger-channel`  Every common message from bot will be sent here.\n" +
                $"4. `/setup on-boarding-file`  Whenever a user joins your server, this file with welcome message will be sent to him/her and in logger-channel.\n" +
                $"5. `/setup prefix` You will change the default prefix for your server.\n" +
                $"6. `/setup authentication` Please choose the channel which only `Unverified` users can see.\n" +
                $"7. `/setup birthday-message` Make a new Birthday message which will be the default message for birthday message.\n" +
                $"8. `/setup welcome-message` Make a new Welcome message which will be the default message for Welcome new user message.\n" +
                $"9. `/admin roles sync` All the server users will need to authenticat with server password. Bot will assign `Unverified` Role to all of users which don't have `Verified` Role\n" +
                $"10. `/admin role add/remove` Admin can Assign or remove roles to/from users.\n" +
                $"11. `/hr role add/remove` HR can assign or remove roles to/from users.\n" +
                $"12. `/hr user-send file` HR can send onboarding file to specific user\n" +
                $"13. `/birthday add` Users can add their birthday date, the bot will then announce in logger channel.\n" +
                $"14. `/external account` Users can add their external account Id (jira, bitbucket).\n" +
                $"15. `/scrin invite` Only the admin of this sever can run this command to invite the user to scirn.io.\n"
                ).Build();
            await RespondAsync(embed: embed);
        }

        // The command that Admin make it once and then all new user will use it to enter thier password
        // for authentication
        [SlashCommand("authentication", "setup authentication channel")]
        [RequireGuildSetup(RequireGuildSetup.GuildSetupType.RoleId)]
        public async Task authentication(ITextChannel channel)
        {
            await DeferAsync(true);

            var embed = new EmbedBuilder()
            {
                Title = "Authentication",
                Description = "Specify your identity, so that the necessary accesses are issued.",
                ThumbnailUrl = "https://img.icons8.com/fluency/344/identification-documents.png",
            };
            var component = new ComponentBuilder()
                .WithButton("Sign In", "button-admin-setup-authentication-password", ButtonStyle.Success, new Emoji("🔒"), null, false, 0);

            await channel.SendMessageAsync(embed: embed.Build(), components: component.Build());

            await FollowupAsync("The channel updated, make sure the channel is private and unwriteable.", ephemeral: true);
        }

    }
}
