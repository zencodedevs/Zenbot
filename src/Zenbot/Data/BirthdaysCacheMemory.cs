using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zenbot.Data
{
    using ServerId = String;
    using UserId = String;

    /// <summary>
    /// Implementation of IBirthdayCache where cache is stored in memory
    /// </summary>
    public class BirthdaysCacheMemory : IBirthdaysCache // IBirthdaysRepositoryCached
    {
        // Since I will need to look up Users from Birthdays, which is a
        // many-to-one relation, List makes more sense than a Dictionary
        private List<Birthday> _birthdaysCache = new();

        // Separately stored list of User IDs for fast duplicate check
        private HashSet<string> _userIdsCache = new();

        private bool IsUserDuplicate(UserId userId)
        {
            if (_userIdsCache.Contains(userId))
                return true;
            else
                return false;
        }

        protected void AddUserBirthdayInternalStorage(Birthday birthday)
        {
            _birthdaysCache.Add(birthday);
            _userIdsCache.Add(birthday.UserId);
        }

        public async Task AddUserBirthdayAsync(Birthday birthday)
        {
            if (IsUserDuplicate(birthday.UserId))
                throw new ArgumentException($"Failed to add a new User Birthday. The following UserId already exists: {birthday.UserId}");
            else
            {
                AddUserBirthdayInternalStorage(birthday);
                // await SaveToSourceAsync();
            }
        }

        public async Task DeleteUserBirthdayAsync(Birthday birthday) // TBU
        {
            // TBU
        }

        public async Task AdjustUserBirthdayAsync(Birthday birthday) // TBU
        {
            // TBU
        }

        public async Task<List<UserId>> LookupUsersByBirthdayAsync(DateTime birthdayDate, ServerId serverId = null)
        {
            List<UserId> users = _birthdaysCache
                .Where(birthday => birthday.BirthdayDate.Date == birthdayDate.Date
                                && birthday.ServerId == serverId)
                .Select(birthday => birthday.UserId)
                .ToList<UserId>();

            return users;
        }
    }
}
