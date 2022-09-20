using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Entities.Zenbot;

namespace Zenbot.Services.Jira
{
    public class JiraService
    {
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _client;
        private readonly UsersService _usersService;
        private readonly BotConfiguration _config;
        public JiraService(IServiceProvider services)
        {
            _services = services;
            _client = services.GetRequiredService<DiscordSocketClient>();
            _usersService = services.GetRequiredService<UsersService>();
            _config = services.GetRequiredService<BotConfiguration>();

            _client.Ready += _client_Ready;
        }

        private Task _client_Ready()
        {
            return Task.CompletedTask;
        }
    }
}

