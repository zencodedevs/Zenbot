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

    

        public async Task<int> GetVocationAmountAsync(int requestId)
        {
            var currentYear = DateTime.UtcNow.Year;

            var vocations = await base.GetManyAsync(x=> x.UserRequestId 
                          == requestId && x.IsAccept && x.StartDate.Year == currentYear);

            var currentMonth = DateTime.UtcNow.Month;

            int vocation = 0;

            foreach (var item in vocations)
            {
                vocation += Convert.ToInt32((item.EndDate - item.StartDate).TotalDays);
            }

            vocation = currentMonth - vocation;
          
            return vocation;
        }

        public async Task<int> AddVocationAsync(int requestId, DateTime startDate, DateTime endDate, int supervisorId)
        {
                var vocation = new Vocation
                {
                    RequestDate = DateTime.UtcNow,
                    UserRequestId = requestId,
                    StartDate = startDate,
                    EndDate = endDate,
                    SupervisorId = supervisorId,
                    IsAccept = false
                };

               var NewVctn= await base.InsertAsync(vocation);
            return NewVctn.Id;
        }
    }
}
