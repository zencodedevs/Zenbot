using BotCore.Entities;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace BotCore.Interactions.Modules
{
    public class PublicModule : InteractionModuleBase<CustomSocketInteractionContext>
    {

        [SlashCommand("ping", "Ping the bot")]
        public async Task ping()
        {
            await DeferAsync();
            await FollowupAsync($"Pong ! ` Current Latency {(Context.Client as DiscordSocketClient).Latency}`");
        }
    }
}
