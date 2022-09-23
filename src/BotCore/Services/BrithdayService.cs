using BotCore;
using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Uow;


namespace Zenbot.BotCore
{
    public class BrithdayService
    {
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _client;
        private readonly UsersService _usersService;
        private readonly BotConfiguration _config;
        public BrithdayService(IServiceProvider services)
        {
            _services = services;
            _client = services.GetRequiredService<DiscordSocketClient>();
            _usersService = services.GetRequiredService<UsersService>();
            _config = services.GetRequiredService<BotConfiguration>();

            _client.Ready += _client_Ready;
        }

        private Task _client_Ready()
        {
            _ = Task.Run(async () =>
            {


                // while loop for calling the function everyday
                while (true)
                {
                    var users = await _usersService.GetUpComingUsersBrithday();
                    if (!users.IsNullOrEmpty())
                    {
                        try
                        {
                            foreach (var u in users)
                            {
                                try
                                {
                                    await _client.GetGuild(_config.MainGuildId).GetTextChannel(_config.Channels.LoggerId)
                                          .SendMessageAsync($"@everyone Congrates <@{u.UserId}>'s birthday, Happy Birtday 🎉");

                                    u.NextNotifyTIme = DateTime.UtcNow
                                    .AddYears(1)
                                    .AddSeconds(-30);
                                }
                                catch
                                {

                                }
                                finally
                                {
                                    await Task.Delay(200);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        await Task.Delay(TimeSpan.FromSeconds(30));
                    }
                }
            });
            return Task.CompletedTask;
        }
    }
}
