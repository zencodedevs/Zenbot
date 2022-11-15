using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Interfaces;

namespace Zenbot.Infrastructure.Shared.Services
{
    public class VocatoinServices : IVocationServices
    {
        private readonly IEntityFrameworkRepository<Vocation> _repository;
        public VocatoinServices(IEntityFrameworkRepository<Vocation> repository)
        {
            _repository = repository;
        }
        public async Task<List<Vocation>> GetVocationListByGuildId(int guildId)
        {
            var query = await _repository.GetQueryableAsync(x => x.Guild, x=> x.UserRequest, x=> x.Supervisor);
            var vocations = await query.Where(x => x.GuildId == guildId).ToListAsync();
            return vocations;
        }
    }
}
