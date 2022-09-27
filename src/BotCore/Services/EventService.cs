using BotCore;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;

namespace Zenbot.BotCore
{
   /// <summary>
   /// All the event which is for Discord Server whenver some event happans
   /// </summary>
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

                await SendMessageToLoggerChannelAsync($"Can not send message to <@{targetId}>:\n" +
                    $"1. make sure user direct is open.\n" +
                    $"2. make sure user have a joined this server.");

            }
        }


        private async Task _client_Ready()
        {
            var serversCount = _client.Guilds.Count;
            var memebrsCount = _client.Guilds.Sum(a => a.MemberCount);
            var mainServer = _client.GetGuild(_config.MainGuildId);
            var embed = new EmbedBuilder()
            {
                Title = "I am ready",
                Description =
                $"Hi, i am online, my current latency to discord is **{_client.Latency}ms**!\n" +
                $"Total servers: `{serversCount}`\n" +
                $"Total members: `{memebrsCount}`\n" +
                $"{mainServer.Name}'s members: `{memebrsCount}`\n" +
                $"{mainServer.Name}'s roles: `{mainServer.Roles.Count}`\n" +
                $"{mainServer.Name}'s channels: `{mainServer.Channels.Count}`",
                ThumbnailUrl = "https://img.icons8.com/fluency/344/chatbot.png",
                Color = 7976191
            }.Build();

            await SendMessageToLoggerChannelAsync(embed: embed);


            var guild = _client.GetGuild(_config.MainGuildId);
            var users = await GuildRolesManagment.GetUsersWithoutAnyRoleAsync(guild, _config.Roles.VarifiedId, _config.Roles.UnVarifiedId);
            await GuildRolesManagment.SyncMemberRolesAsync(users, _config.Roles.UnVarifiedId);
        }

        private async Task _client_UserJoined(SocketGuildUser user)
        {
            await user.AddRoleAsync(_config.Roles.UnVarifiedId);
        }

        public async Task SendMessageToLoggerChannelAsync(string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null, AllowedMentions allowedMentions = null, MessageReference messageReference = null, MessageComponent components = null, ISticker[] stickers = null, Embed[] embeds = null, MessageFlags flags = MessageFlags.None) =>
            await _client.GetGuild(_config.MainGuildId).GetTextChannel(_config.Channels.LoggerId).SendMessageAsync(text, isTTS, embed, options, allowedMentions, messageReference, components, stickers, embeds, flags);

    }
}