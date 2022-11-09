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
    public class GuildService : IGuildService
    {
        private readonly IEntityFrameworkRepository<Guild> _repository;
        public GuildService(IEntityFrameworkRepository<Guild> repository)
        {
            _repository = repository;
        }
        public Task<IEnumerable<Guild>> GetGuildsByUserId(ulong userId)
        {
            throw new NotImplementedException();
        }
    }
}
