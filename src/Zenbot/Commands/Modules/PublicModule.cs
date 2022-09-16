using Discord.Commands;
using System.Threading.Tasks;

namespace Zenbot.Commands.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        [Command("Hello")]
        public async Task ping()
        {
            await ReplyAsync($"Hello dear {Context.User.Username}");
        }
    }
}