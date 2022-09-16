using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Zenbot.Entities.Zenbot;

namespace Zenbot
{
    public class EventService
    {
        private readonly BotConfiguration _config;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider services;
        public EventService(IServiceProvider services)
        {
            this.services = services;
            this._config = services.GetRequiredService<BotConfiguration>();
            this._client = services.GetRequiredService<DiscordSocketClient>();

            this._client.UserJoined += _client_UserJoined;
            this._client.Ready += _client_Ready;

        }

        private async Task _client_Ready() => await SendMessageToLoggerChannel("I am online.");
        private async Task _client_UserJoined(SocketGuildUser user) => await SendMessageToLoggerChannel($"Say Welcome To {MentionUtils.MentionUser(user.Id)}");

        public async Task SendMessageToLoggerChannel(string text) =>
            await _client.GetGuild(_config.MainGuildId).GetTextChannel(_config.LoggerChannel).SendMessageAsync(text);

    }
}