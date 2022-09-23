using Discord.Commands;
using System.Threading.Tasks;

namespace Zenbot.BotCore.Commands.Modules
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
    }
}