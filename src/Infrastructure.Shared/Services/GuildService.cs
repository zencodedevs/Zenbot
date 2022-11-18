using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zenbot.Domain.Shared.Common;
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

        public async Task<Guild> UpdateGSuiteAuthForGuild(int guildId, IFormFile gsuite)
        {
            var guild = await _repository.FindAsync(x => x.Id == guildId);
            if (guild != null)
            {
                UploadFiles.DeleteImg(guild.GSuiteAuth);

                string imgname = UploadFiles.CreateImg(gsuite, "gsuite");

                guild.GSuiteAuth = "bot/gsuite/" + imgname;

                await _repository.UpdateAsync(guild);
                await _repository.SaveChangesAsync(true);
                return guild;
            }
            return guild;
        }

        public async Task<Guild> UpdatePasswordForGuild(int guildId, string password)
        {
            var guild = await _repository.FindAsync(x => x.Id == guildId);
            if (guild is not null)
            {
                guild.AuthenticationPassword = password;
                await _repository.UpdateAsync(guild);
                await _repository.SaveChangesAsync(true);
                return guild;
            }
            return guild;
        }

        public async Task<Guild> UpdateScrinIOForGuild(int guildId, string scrinio)
        {
            var guild = await _repository.FindAsync(x => x.Id == guildId);
            if (guild is not null)
            {
                guild.ScrinIOToken = scrinio;
                await _repository.UpdateAsync(guild);
                await _repository.SaveChangesAsync(true);
                return guild;
            }
            return guild;
        }
    }
}
