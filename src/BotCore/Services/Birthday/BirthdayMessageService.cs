using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Services
{
    public class BirthdayMessageService : EntityBaseService<BirthdayMessage>
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;


        public BirthdayMessageService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }


        public async Task<BirthdayMessage> GetOrAddAsync(bool isActive, string message, int guildId)
        {

            var birthdayMessage = await base.GetAsync(a => a.GuildId == guildId);
            if (birthdayMessage is not null)
            {
                birthdayMessage = await base.UpdateAsync(birthdayMessage, x =>
                {
                    x.Message = message;
                    x.IsActive = isActive;
                });
                return birthdayMessage;
            }

            var newMessage = new BirthdayMessage()
            {
                IsActive = isActive,
                Message = message,
                GuildId = guildId
            };
            var result = await base.InsertAsync(newMessage);
            return result;

        }

        public async Task<bool> CheckIfBirthdayMessageIsEnable(int guildId)
        {
            var message = await base.GetAsync(a => a.GuildId == guildId && a.IsActive);
            if(message == null) return false;
            return true;
        }
    }
}
