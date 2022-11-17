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

        public async Task<BotUser> GetBotUserById(int userId)
        {
            return await _repository.FindAsync(x=> x.Id == userId);
        }

        public async Task<bool> UpdateBirthday(DateTime date, int userId)
        {
            var user = await _repository.FindAsync(x => x.Id == userId);
            if (user is not null)
            {
                user.Birthday = date;
                await _repository.UpdateAsync(user);
                await _repository.SaveChangesAsync(true);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateIntegration(string jiraAccount, string bitBucketAccount, bool isEnable, int userId)
        {
            var user = await _repository.FindAsync(x => x.Id == userId);
            if (user is not null)
            {
                user.JiraAccountID = jiraAccount;
                user.BitBucketAccountId = bitBucketAccount;
                user.IsEnableIntegration = isEnable;
                await _repository.UpdateAsync(user);
                await _repository.SaveChangesAsync(true);
                return true;
            }
            return false;
        }
    }
}
