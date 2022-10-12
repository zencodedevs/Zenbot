using Discord.WebSocket;
using Domain.Shared.Entities.Bot;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Services
{
    public class WelcomeMessageService : EntityBaseService<WelcomeMessage>
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;


        public WelcomeMessageService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }


        public async Task<WelcomeMessage> GetOrAddAsync(bool isActive, string message, int guildId)
        {
            if (isActive)
            {
                var welcomeMessage = await base.GetAsync(a => a.IsActive);
                if (welcomeMessage is not null)
                {
                    welcomeMessage = await base.UpdateAsync(welcomeMessage, x => x.IsActive = false);
                }
            }

            var newMessage = new WelcomeMessage()
            {
                IsActive = isActive,
                Message = message,
                GuildId = guildId
            };
            var result = await base.InsertAsync(newMessage);
            return result;

        }
    }
}
