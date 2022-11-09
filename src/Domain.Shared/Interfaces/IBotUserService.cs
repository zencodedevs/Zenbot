using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Domain.Shared.Interfaces
{
    public interface IBotUserService
    {
        Task<BotUser> GetBotUserByDiscordId(ulong discordId);
    }
}
