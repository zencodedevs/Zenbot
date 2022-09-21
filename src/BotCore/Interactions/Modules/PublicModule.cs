using Discord.Interactions;
using System.Threading.Tasks;
namespace BotCore.Interactions.Modules
{
    public class PublicModule : InteractionModuleBase<SocketInteractionContext>
    {

        [SlashCommand("ping", "Ping the bot")]
        public async Task ping()
        {
            
            await DeferAsync();
        }
    }
}
