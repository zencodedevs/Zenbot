using Discord.Interactions;
using System.Threading.Tasks;
namespace Zenbot.Interactions.Modules
{
    public class PublicModule : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("ping", "Ping the bot")]
        public async Task ping()
        {
            await RespondAsync("done");
        }
    }
}
