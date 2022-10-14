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
    public class SupervisorService : EntityBaseService<SupervisorEmployee>
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;


        public SupervisorService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }


        public async Task<SupervisorEmployee> GetOrAddAsync(int supervisor, int user)
        {
            var newMessage = new SupervisorEmployee()
            {
               EmployeeId = user,
               SupervisorId = supervisor,
            };
            var spr = await base.GetAsync(x=> x.EmployeeId == user && x.SupervisorId == supervisor);
            if(spr == null)
            {
                var result = await base.InsertAsync(newMessage);
                return result;
            }
            return null;

        }

        public async Task<SupervisorEmployee> GetSupervisor(int requestId)
        {
            return await base.GetAsync(x => x.EmployeeId == requestId);
        }
    }
}
