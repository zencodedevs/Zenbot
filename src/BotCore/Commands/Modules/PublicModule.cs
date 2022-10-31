using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace BotCore.Commands.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        [Command("Hello")]
        public async Task hello()
        {
            await ReplyAsync($"Hello dear {Context.User.Username}");
        }

        [Command("ping")]
        public async Task ping()
        {
            await ReplyAsync($"Pong ! ` Current Latency {Context.Client.Latency}`");
        }

        [Command("help")]
        public async Task help()
        {
            var helpEmbed = new EmbedBuilder()
                .WithTitle("Help Commands")
                .WithDescription($"Need help ? please consider the following steps! \n\n " +
                $"1. `birthday-add` Run this command so you will be registerd in this server and also bot will notify your birthday on server \n" +
                $"2. `external account` Run this command and register your external account Id like `Jira and Bitbucket` you'll recieve message whenever somone assigns you a task or make you a reviewer on a pull request\n" +
                $"3. `vocation request` Run this command to request for vocation. You'll recieve message whenever your supervisor accept or decline \n" +
                $"4. `vocation list` You will get the list of the requests you made for vocation"
                ).Build();

            await ReplyAsync(embed: helpEmbed);
           
        }
    }
}