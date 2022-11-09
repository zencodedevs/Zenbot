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
        public SupervisorService _supervisorService { get; set; }
        public BotUserGuildServices _botUserGuildServices { get; set; }
        public GuildService guildService { get; set; }




        // Setup the Supervisor for this Guild
        // we can have multiple Supervisor

        [SlashCommand("select-supervisor", "Choose a user to be supervisor")]
        public async Task supervisor(IUser supervisor)
        {
            await DeferAsync();
            var user = await _userService.GetUserByDiscordId(supervisor.Id);
            var newSpr = _userService.UpdateAsync(user, x =>
            {
                x.IsSupervisor = true;
            });

            await FollowupAsync($"Great! you have made `{user.Username}` as a `Supervisor`");

            // Log the message
            var message = $"Selected `{supervisor.Username}` as a Supervisor";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }



        // Command to show list of all supervisor of this Guild
        [SlashCommand("list-supervisor", "show list of all supervisor in this servisor")]
        public async Task listSupervisor()
        {
            await DeferAsync();
            var currenGuildUsers = await _botUserGuildServices.GetUsersByGuildId(Context.BotGuild.Id);
            var sprs = await _userService.GetSupervisorersForCurrentGuild(currenGuildUsers);

            var embedBuiler = new EmbedBuilder()
            .WithTitle("List of all Supervisors in this Server")
            .WithColor(Color.Purple)
            .WithCurrentTimestamp();

            foreach (var item in sprs)
            {
                embedBuiler.AddField("Name", item.Username, true);
            }

            await FollowupAsync(embed: embedBuiler.Build(), ephemeral: true);

            // Log the message
            var message = $" Requested for list of supervisors";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }


        // Replace the default bot prefix with your own profex
        [SlashCommand("assign-supervisor", "You can assign supervisor to selected user")]
        public async Task AssignSupervisor(IUser supervisor, IUser user)
        {
            await DeferAsync();

            var spr = await _userService.GetUserByDiscordId(supervisor.Id);
            if (!spr.IsSupervisor)
            {
                await FollowupAsync($"`{spr.Username}` is not Supervisor. make sure first run `setup select-supervisor` command and try again");
                return;
            }
            var emp = await _userService.GetUserByDiscordId(user.Id);

            if(spr != null && emp != null)
            {
              var insert =  await _supervisorService.GetOrAddAsync(spr.Id, emp.Id);
                if(insert == null)
                {
                    await FollowupAsync($"You've already selected `{spr.Username}` as Supervisor for `{emp.Username}`");
                    return;
                }

                await FollowupAsync($"Great! You've selected `{spr.Username}` as supervisor for `{emp.Username}`.", null, false,ephemeral:true);
                return;
            }

            await FollowupAsync("Supervisor or user is not registerd in database!");

            // Log the message
            var message = $"Selected `{supervisor.Username}` as Supervisor for `{user.Username}`";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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

            // Log the message
            var message = $"Created a password for server : {password}";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }


        // setup the scrin io token for this server
        [SlashCommand("scrin-io-token", "setup scrin io tokne bot will use that to invite users")]
        public async Task scrin_io(string token)
        {
            await DeferAsync();
            await Context._guildService.UpdateAsync(Context.BotGuild.Id, x =>
            {
                x.ScrinIOToken = token;
            });
            await FollowupAsync($"The setup for scrin io token done succesfuly.");

            // Log the message
            var message = $"Insert the ScrinIO Auth Token : `{token}`";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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

            // Log the message
            var message = $"Selected `{Context.Channel.Name}` as logger channel";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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

            // Log the message
            var message = $"Created a birthday message : `{form.Message}`";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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

            // Log the message
            var message = $"Created a welcome message : `{form.Message}`";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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

            // Log the message
            var message = $"Roles set up for server: `{verified.Name}, {unVerified.Name}, {hr.Name}`";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }

        // Set the greeting message for this Guild
        [SlashCommand("on-boarding", "setup server's on-boarding file")]
        public async Task onBoardingFile(IAttachment grettingFile = null)
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

            // Download the file from Discord and put in server root directory
            // then we can just sent this file from discord

            using (WebClient client = new WebClient())
            {
                var directoryPath = $@"BotFiles/guilds/{Context.Guild.Id}/";
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
                    await FollowupAsync("on boarding file updated for your server.");

                    // Log the message
                    var message = $"On boarding file created : `{filePath}`";
                    await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

                }

                client.DownloadFileAsync(new Uri(grettingFile.Url), filePath);
                await FollowupAsync("please wait, we are downloading the file.");
            }
        }




        // Set the g suite auth credentials for this Guild
        [SlashCommand("gsuite-auth", "setup server's gsuite credentials.")]
        public async Task gsuit_auth(IAttachment gsuiteAuth = null)
        {
            await DeferAsync();

            if (File.Exists(Context.BotGuild.GSuiteAuth))
                File.Delete(Context.BotGuild.GSuiteAuth);

            if (gsuiteAuth is null)
            {
                await Context._guildService.UpdateAsync(Context.BotGuild, x =>
                {
                    x.GreetingFilePath = "";
                });
                return;
            }

            // Download the file from Discord and put in server root directory
            // then we can just sent this file from discord

            using (WebClient client = new WebClient())
            {
                var directoryPath = $@"BotFiles/gsuite/{Context.Guild.Id}/";
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                var filePath = directoryPath + gsuiteAuth.Filename;
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
                        x.GSuiteAuth = filePath;
                    });
                    await FollowupAsync("G Suite credentials updated for your server.");

                    // Log the message
                    var message = $"G Suite credentials updated : `{filePath}`";
                    await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

                }

                client.DownloadFileAsync(new Uri(gsuiteAuth.Url), filePath);
                await FollowupAsync("please wait, we are downloading the file.");
            }

        }




        //Help Command for the whole bot setup
        [SlashCommand("help", "help")]
        public async Task help()
        {
            await DeferAsync(true);

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
                $"4. `/setup on-boarding`  Whenever a user joins your server, this file with welcome message will be sent to him/her and in logger-channel.\n" +
                $"5. `/setup gsuite-auth`  Please upload the json file you get from google console app, the project shoud be `installed` application in google console app.\n" +
                $"6. `/gsuite create-account`  For creating a g suite user account in google.\n" +
                $"7. `/setup authentication` Please choose the channel which only `Unverified` users can see.\n" +
                $"8. `/setup birthday-message` Make a new Birthday message which will be the default message for birthday message.\n" +
                $"9. `/setup welcome-message` Make a new Welcome message which will be the default message for Welcome new user message.\n" +
                $"10. `/admin roles sync` All the server users will need to authenticat with server password. Bot will assign `Unverified` Role to all of users which don't have `Verified` Role\n" +
                $"11. `/admin role add/remove` Admin can Assign or remove roles to/from users.\n" +
                $"12. `/hr role add/remove` HR can assign or remove roles to/from users.\n" +
                $"13. `/hr user-send file` HR can send onboarding file to specific user\n" +
                $"14. `/birthday add` Users can add their birthday date, the bot will then announce in logger channel.\n" +
                $"15. `/external account` Users can add their external account Id (jira, bitbucket).\n" +
                $"16. `/setup select-supervisor` You can choose a user and register that as supervisor.\n" +
                $"17. `/setup list-supervisor` You will get all of your supervisors list you registerd in this server\n" +
                $"18. `/setup assign-supervisor` Here you can assign a user as supervisor for other user.\n" +
                $"19. `/setup scrionio-token` you can run register your scrin io for this server and user `scrin invite` command to invite people.\n" +
                $"20. `/scrin invite` Only the admin of this sever can run this command to invite the user to scirn.io.\n"
                ).Build();

            await FollowupAsync(embed: embed);

            // add User to database
            await Context._botUserGuildServices.UpdateAsync(Context.BotUserGuild, x =>
            {
                x.BotUserId = Context.BotUser.Id;
                x.GuildId = Context.BotGuild.Id;
                x.IsAdmin = true;
            });

            await guildService.UpdateAsync(Context.BotGuild, x =>
            {
                x.GuildName = Context.Guild.Name;
            });
            // Log the message
            var message = $"Admin help command ran";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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

            // Log the message
            var message = $"Authentication command ran by admin";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }

    }
}
