using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;

namespace Zenbot.Domain.Shared.Interfaces
{
    public interface IBotUserGuildService
    {
        Task<List<BotUserGuild>> GetAllGuildsByUserId(ulong userId);
    }
}
