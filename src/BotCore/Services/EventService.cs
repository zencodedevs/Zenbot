using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using BotCore.Entities.BotCore;

namespace BotCore
{
    public class EventService
    {
        private readonly BotConfiguration _config;
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider services;
        public EventService(IServiceProvider services = null)
        {
            this.services = services;
            this._config = services.GetRequiredService<BotConfiguration>();
            this._client = services.GetRequiredService<DiscordSocketClient>();


            this._client.UserJoined += _client_UserJoined;
            this._client.Ready += _client_Ready;

        }

        public async Task SendMessageToUserByJiraId(string jiraId)
        {
            var userService = services.GetRequiredService<UsersService>();
            var target = await userService.GetUserByJiraId(jiraId);

            if (target is null)
                return;

            var targetId = target.UserId;
            var user = await _client.GetUserAsync(targetId);

            try
            {
                await user.SendMessageAsync($"<@{targetId}> you have new message.");
            }
            catch
            {

                await SendMessageToLoggerChannel($"Can not send message to <@{targetId}>:\n" +
                    $"1. make sure user direct is open.\n" +
                    $"2. make sure user have a joined this server.");

            }
        }


        private async Task _client_Ready() => await SendMessageToLoggerChannel("I am online.");
        private async Task _client_UserJoined(SocketGuildUser user) => await SendMessageToLoggerChannel($"Say Welcome To {MentionUtils.MentionUser(user.Id)}");

        public async Task SendMessageToLoggerChannel(string text) =>
            await _client.GetGuild(_config.MainGuildId).GetTextChannel(_config.LoggerChannel).SendMessageAsync(text);

    }
}