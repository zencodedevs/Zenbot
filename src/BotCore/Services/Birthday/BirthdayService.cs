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
    public class BirthdayService
    {
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _discord;
        private readonly UserService _usersService;
        private readonly BotConfiguration _config;
        private readonly GuildService _guildService;
        private readonly ChannelService _channelService;
        public BirthdayService(IServiceProvider services)
        {
            _services = services;
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _usersService = services.GetRequiredService<UserService>();
            _config = services.GetRequiredService<BotConfiguration>();
            _guildService = services.GetRequiredService<GuildService>();
            _channelService = services.GetRequiredService<ChannelService>();
            _discord.Ready += _client_Ready;
        }

        // Method which will be called every 24 hours to check if there are in bot servers who has birthday
        private Task _client_Ready()
        {
            _ = Task.Run(async () =>
            {
                // while loop for calling the function everyday
                while (true)
                {
                    var users = await _usersService.GetUpComingUsersBrithday();
                    await NotficationUsersBirthdayAsync(users);

                    await Task.Delay(TimeSpan.FromHours(24));
                }
            });

            return Task.CompletedTask;
        }



        public async Task NotficationUsersBirthdayAsync(ICollection<BotUser> users)
        {
            if (users is null || users.Count < 1)
                return;

            List<Guild> botGuilds = new();
            List<GuildChannel> botChannels = new();

            var listUsers = _discord.Guilds.SelectMany(a => a.Users);

            foreach (var u in users)
            {
                SocketUser user = listUsers.FirstOrDefault(a => a.Id == u.DiscordId);

                if (user is null || user.MutualGuilds is null || user.MutualGuilds.Count < 1)
                    continue;

                foreach (var guild in user.MutualGuilds)
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

                    // Getting message from database the one which is active
                    var birthday_message = await _guildService.GetBirthdayMessageAsync(botGuild.Id);
                    // Message text and replace the {username} with Discord username
                    var bMessage = birthday_message.Message.Replace("{username}", $"<@{u.DiscordId}>");

                    var brithday_embed = new EmbedBuilder()
                    {
                        Title = "Happy Birthday",
                        Description = $"{bMessage} \n\n  @everyone  ",
                        Color = Color.Purple,
                        ThumbnailUrl = "https://img.icons8.com/external-flat-icons-pause-08/64/000000/external-birthday-christmas-collection-flat-icons-pause-08.png",
                    }.Build();
                    await _channelService.SendMessageAsync(loggerChannel.ChannelId, null,false,embed: brithday_embed);

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