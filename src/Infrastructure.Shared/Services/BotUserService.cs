using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zenbot.Domain.Shared.Interfaces;

namespace Zenbot.Infrastructure.Shared.Services
{
    public class BotUserService : IBotUserService
    {
        private readonly IEntityFrameworkRepository<BotUser> _repository;
        public BotUserService(IEntityFrameworkRepository<BotUser> repository)
        {
            _repository = repository;
        }
        public async Task<BotUser> GetBotUserByDiscordId(ulong discordId)
        {
            return await _repository.FindAsync(x => x.DiscordId == discordId);
           
        }
    }
}
