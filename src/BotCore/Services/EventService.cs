using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Zenbot.BotCore.Entities.Zenbot;
using Zenbot.BotCore.Models;

namespace Zenbot.BotCore.Services
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

            var botService = services.GetRequiredService<DiscordBotService>();
            botService.OnRecevied += BotService_OnRecevied;

            this._client.UserJoined += _client_UserJoined;
            this._client.Ready += _client_Ready;

        }

        private async Task BotService_OnRecevied(string jiraId)
        {
            var userService = services.GetRequiredService<UsersService>();
            var targetUser = await userService.GetUserByJiraId(jiraId);

            if (targetUser is null)
                return;

            var targetUserId = targetUser.UserId;
            var user = await _client.GetUserAsync(targetUserId);

            try
            {
                await user.SendMessageAsync($"<@{targetUserId}> you have new message.");
            }
            catch
            {

                await SendMessageToLoggerChannel($"Can not send message to <@{targetUserId}>:\n" +
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