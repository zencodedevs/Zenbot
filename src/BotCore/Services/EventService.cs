using BotCore.Entities;
using BotCore.Utilities;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Services
{
    /// <summary>
    /// All the event which is for Discord Server whenver some event happans
    /// </summary>
    public class EventService
    {
        private readonly BotConfiguration _config;
        private readonly DiscordSocketClient _client;
        private readonly GuildService _guildService;
        private readonly ChannelService _channelService;
        private readonly IServiceProvider services;
        public EventService(IServiceProvider services = null)
        {
            this.services = services;
            _config = services.GetRequiredService<BotConfiguration>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _guildService = services.GetRequiredService<GuildService>();
            _channelService = services.GetRequiredService<ChannelService>();

            _client.UserJoined += _client_UserJoined;
            _client.Ready += _client_Ready;
            _client.JoinedGuild += _client_JoinedGuild;
            _client.LeftGuild += _client_LeftGuild;
        }

        private Task _client_LeftGuild(SocketGuild guild)
        {
            return Task.CompletedTask;
        }

        private async Task _client_JoinedGuild(SocketGuild guild)
        {
            var bGuild = await _guildService.GetOrAddAsync(guild.Id);

            var embed = new EmbedBuilder()
                .WithTitle("")
                .WithDescription($"The bot joined your server **{guild.Name}**\n")
                .AddField("Setup",
                          $"To setup the bot, use the command" +
                          $"```/setup server```", false)
                .WithThumbnailUrl(_client.CurrentUser.GetAvatarUrl() ?? _client.CurrentUser.GetDefaultAvatarUrl())
                .WithColor(Color.Green)
                .Build();

            await guild.Owner.SendMessageAsync(embed: embed);
        }

        private async Task _client_Ready()
        {
            var serversCount = _client.Guilds.Count;
            var memebrsCount = _client.Guilds.Sum(a => a.MemberCount);
            var embed = new EmbedBuilder()
            {
                Title = "I am ready",
                Description =
                $"Hi, i am online, my current latency to discord is **{_client.Latency}ms**!\n" +
                $"Total servers: `{serversCount}`\n" +
                $"Total members: `{memebrsCount}`\n",
                ThumbnailUrl = "https://img.icons8.com/fluency/344/chatbot.png",
                Color = 7976191
            }.Build();

            var guild = await _guildService.GetAsync(a => a.IsMainServer);

            if (guild is not null)
            {
                var loggerChannel = await _guildService.GetChannelAsync(guild.Id, GuildChannelType.Logger);
                await _channelService.SendMessageAsync(loggerChannel.ChannelId, embed: embed);
            }

            await _guildService.SyncAllGuildsUsersRolesAsync();
        }

        private async Task _client_UserJoined(SocketGuildUser user)
        {
            var guild = await _guildService.GetAsync(a => a.GuildId == user.Guild.Id);
            if (guild is not null && guild.UnVerifiedRoleId != 0)
            {
                await user.AddRoleAsync(guild.UnVerifiedRoleId);
            }
        }
    }
}