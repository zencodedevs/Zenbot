using BotCore.Services;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace BotCore.Commands.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        public BotUserGuildServices _botUserGuildServices { get; set; }
        public UserService userService { get; set; }
        public GuildService guildService { get; set; }
        public ChannelService _channelService { get; set; }

        [Command("Hello")]
        public async Task hello()
        {
            await ReplyAsync($"Hello dear `{Context.User.Username}`, this is Zenbot! \n Thank you for joining me, I will be your personal partner from now!");

            // Log the message
            var message = $"Hello Command ran";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

            // Add Server and discord user into database
            var guild = await guildService.GetOrAddAsync(Context.Guild.Id);
            var user = await userService.GetOrAddAsync(Context.User.Id, Context.User.Username);
            await _botUserGuildServices.GetOrAddAsync(guild.Id, user.Id, false);
            
            // Add server name
            await guildService.UpdateAsync(guild.Id, x =>
            {
                x.GuildName = Context.Guild.Name;
            });
        }



        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireContext(ContextType.Guild)]
        //Help Command for the whole bot setup
        [Command("setup-help")]
        public async Task SetupHelp()
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

            await ReplyAsync(embed: embed);

            //    // Add Server and discord user into database
            var guild = await guildService.GetOrAddAsync(Context.Guild.Id);
            var user = await userService.GetOrAddAsync(Context.User.Id, Context.User.Username);
            await _botUserGuildServices.GetOrAddAsync(guild.Id, user.Id, true);

            // Add server name
            await guildService.UpdateAsync(guild.Id, x =>
            {
                x.GuildName = Context.Guild.Name;
            });

            // Log the message
            var message = $"Admin help command ran";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }



        [Command("ping")]
        public async Task ping()
        {
            await ReplyAsync($"Pong ! ` Current Latency {Context.Client.Latency}`");
            // Log the message
            var message = $"Ping command ran";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }

        [Command("myId")]
        public async Task MyId()
        {
            await ReplyAsync($"Your Discord ID:  `{Context.User.Id}`");
            // Log the message
            var message = $"Request for discord Id";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }

        [Command("help")]
        public async Task help()
        {
            var helpEmbed = new EmbedBuilder()
                .WithTitle("Help Commands")
                .WithDescription($"Need help? please consider the following steps! \n\n " +
                $"1. `/birthday-add` Run this command `Zenbot` will notify your birthday in general channel \n" +
                $"2. `/external account` Run this command and register your external account Id like `Jira and Bitbucket` you'll recieve message whenever somone assigns you a task or make you a reviewer on a pull request\n" +
                $"3. `/vocation request` Run this command to request for vocation. You'll recieve message whenever your supervisor accept or decline \n" +
                $"4. `/vocation list` You will get the list of the requests you made for vocation \n"+
                $"5. `/setup-help` For setup the bot in the server (Adminstrator permission required)"
                ).Build();

            await ReplyAsync(embed: helpEmbed);

            // Add Server and discord user into database
            var guild = await guildService.GetOrAddAsync(Context.Guild.Id);
            var user = await userService.GetOrAddAsync(Context.User.Id, Context.User.Username);
            await _botUserGuildServices.GetOrAddAsync(guild.Id, user.Id, false);
           
            // Add server name
            await guildService.UpdateAsync(guild.Id, x =>
            {
                x.GuildName = Context.Guild.Name;
            });

            // Log the message
            var message = $"Help commmand ran";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }
    }
}