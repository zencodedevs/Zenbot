using BotCore.Entities;
using BotCore.Services;
using Discord.Interactions;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace BotCore.Interactions.Modules
{
    public class PublicModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public ChannelService _channelService { get; set; }

        [SlashCommand("ping", "Ping the bot")]
        public async Task ping()
        {
            await DeferAsync();
            await FollowupAsync($"Pong ! ` Current Latency {(Context.Client as DiscordSocketClient).Latency}`");

            // Log the message
            var message = $"Ping command ran";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }
    }
}
