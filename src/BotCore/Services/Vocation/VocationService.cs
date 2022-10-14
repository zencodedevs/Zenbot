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
    public class VocationService : EntityBaseService<Vocation>
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;


        public VocationService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }


        public async Task<Vocation> GetOrAddAsync(int supervisor, int user)
        {
            
            return null;

        }
    }
}
