using Discord.Commands;
using System.Threading.Tasks;

namespace Zenbot.Commands.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task ping()
        {
            int i = 0;
            int error = (1 / i);
        }
    }
}