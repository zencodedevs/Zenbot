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
        Task<Guild> UpdateScrinIOForGuild(int guildId, string scrinio);
        Task<Guild> UpdateGSuiteAuthForGuild(int guildId, IFormFile gsuite);
        Task<Guild> UpdatePasswordForGuild(int guildId, string password);
    }
}
