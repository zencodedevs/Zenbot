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
    public class BotUserGuildService : IBotUserGuildService
    {
        private readonly IEntityFrameworkRepository<BotUserGuild> _repository;
        public BotUserGuildService(IEntityFrameworkRepository<BotUserGuild> repository)
        {
            _repository = repository;
        }
        public async Task<List<BotUserGuild>> GetAllGuildsByUserId(int userId)
        {
            var query = await _repository.GetQueryableWithDataFilterAsync(x=> x.BotUserId == userId && x.IsAdmin);
            var data = await query.Include(x => x.Guild).ToListAsync();
            return data;
        }
    }
}
