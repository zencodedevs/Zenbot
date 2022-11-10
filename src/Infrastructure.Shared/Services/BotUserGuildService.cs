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
        private readonly IBotUserService _botUserService;
        public BotUserGuildService(IEntityFrameworkRepository<BotUserGuild> repository, IBotUserService botUserService)
        {
            _repository = repository;
            _botUserService = botUserService;
        }
        public async Task<List<BotUserGuild>> GetAllGuildsByUserId(ulong userId)
        {
            var user = await _botUserService.GetBotUserByDiscordId(userId);
            var query = await _repository.GetQueryableAsync(x => x.Guild);
            var guilds = await query.Where(x => x.BotUserId == user.Id && x.IsAdmin).ToListAsync();
            return guilds;
        }
    }
}
