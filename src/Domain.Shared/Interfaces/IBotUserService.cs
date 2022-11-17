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
        Task<BotUser> GetBotUserById(int userId);
        Task<bool> UpdateBirthday(DateTime date, int userId);
        Task<bool> UpdateIntegration(string jiraAccount, string bitBucketAccount, bool isEnable, int userId);
    }
}
