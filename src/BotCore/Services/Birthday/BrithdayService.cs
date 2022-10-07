using BotCore.Entities;
using BotCore.Services;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Uow;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Services.Birthday
{
    /// <summary>
    /// The service which is sending birthday message for Users in exact day in specified channal
    /// </summary>
    public class BrithdayService
    {
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _discord;
        private readonly UserService _usersService;
        private readonly BotConfiguration _config;
        private readonly GuildService _guildService;
        private readonly ChannelService _channelService;
        public BrithdayService(IServiceProvider services)
        {
            _services = services;
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _usersService = services.GetRequiredService<UserService>();
            _config = services.GetRequiredService<BotConfiguration>();
            _guildService = services.GetRequiredService<GuildService>();
            _channelService = services.GetRequiredService<ChannelService>();
            _discord.Ready += _client_Ready;
        }

        private Task _client_Ready()
        {
            _ = Task.Run(async () =>
            {
                // while loop for calling the function everyday
                while (true)
                {
                    var users = await _usersService.GetUpComingUsersBrithday();
                    await NotficationUsersBirthdayAsync(users);

                    await Task.Delay(TimeSpan.FromSeconds(30));
                }
            });

            return Task.CompletedTask;
        }
        private async Task NotficationUsersBirthdayAsync(ICollection<BotUser> users)
        {
            if (users is null)
                return;

            List<Guild> botGuilds = new();
            List<GuildChannel> botChannels = new();

            foreach (var u in users)
            {
                var user = await _discord.GetUserAsync(u.DiscordId);
                var guilds = (user as SocketUser).MutualGuilds.ToList();

                foreach (var guild in guilds)
                {
                    var botGuild = botGuilds.FirstOrDefault(a => a.GuildId == guild.Id);
                    if (botGuild is null)
                    {
                        botGuild = await _guildService.GetAsync(a => a.GuildId == guild.Id);
                        if (botGuild is null)
                            continue;

                        botGuilds.Add(botGuild);
                    }

                    var loggerChannel = botChannels.FirstOrDefault(a => a.GuildId == botGuild.Id);
                    if (loggerChannel is null)
                    {
                        loggerChannel = await _guildService.GetChannelAsync(botGuild.Id, GuildChannelType.Logger);
                        if (loggerChannel is null)
                            continue;

                        botChannels.Add(loggerChannel);
                    }

                    await _channelService.SendMessageAsync(loggerChannel.ChannelId, $"@everyone Congrates <@{u.DiscordId}>'s birthday, Happy Birtday 🎉");

                    await Task.Delay(150);
                }

                await _usersService.UpdateAsync(u, x =>
                {
                    u.NextNotifyTIme = DateTime.UtcNow
                    .AddYears(1)
                    .AddSeconds(-30);
                });
            }
        }
    }
}
