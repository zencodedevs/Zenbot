using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zenbot.Data
{
    using ServerId = String;
    using UserId = String;

    public interface IBirthdaysRepository
    {
        public Task AddUserBirthdayAsync(Birthday birthday);

        public Task DeleteUserBirthdayAsync(Birthday birthday);

        public Task AdjustUserBirthdayAsync(Birthday birthday);

        public Task<List<UserId>> LookupUsersByBirthdayAsync(DateTime birthdayDate, ServerId serverId = null);
    }
}
