using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Zenbot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("hello!")]
        public async Task Hello(IGuildUser user)
        {
            await ReplyAsync($"Hello {user}");
        }
    }
}
