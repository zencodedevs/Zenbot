using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;

namespace Zenbot.Domain.Shared.Interfaces
{
    public interface IGuildService
    {
        Task<IEnumerable<Guild>> GetGuildsByUserId(ulong userId);
        Task<bool> UpdateScrinIOForGuild(int guildId, string scrinio);
        Task<bool> UpdateGSuiteAuthForGuild(int guildId, IFormFile gsuite);
        Task<bool> UpdatePasswordForGuild(int guildId, string password);
    }
}
