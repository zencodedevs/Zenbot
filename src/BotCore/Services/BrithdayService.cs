using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenbot.BotCore.Entities.Zenbot;
using Zenbot.BotCore.Models;

namespace Zenbot.BotCore.Services
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

            //  _client.Ready += _client_Ready;
        }

        private Task _client_Ready()
        {
            _ = Task.Run(async () =>
            {

                var users = await _usersService.GetUpComingUsersBrithday();

                if (!users.IsNullOrEmpty())
                {
                    // while loop for calling the function everyday
                    while (true)
                    {
                        try
                        {
                            foreach (var u in users)
                            {
                                try
                                {
                                    await _client.GetGuild(_config.MainGuildId).GetTextChannel(_config.LoggerChannel)
                                          .SendMessageAsync($"<@{u.Username}> Hey today is your brithday 🎉");

                                    u.NextNotifyTIme = DateTime.UtcNow
                                    .AddYears(1)
                                    .AddSeconds(-30);
                                }
                                catch
                                {

                                }
                                finally
                                {
                                    await Task.Delay(300);
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
