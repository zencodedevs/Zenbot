using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zenbot.Data
{
    class BirthdaysRepositoryFromDatabase<T> : IBirthdaysRepositoryCached where T : class, IBirthdaysCache
    {
        public Task AddUserBirthdayAsync(Birthday birthday)
        {
            throw new NotImplementedException();
        }

        public Task AdjustUserBirthdayAsync(Birthday birthday)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserBirthdayAsync(Birthday birthday)
        {
            throw new NotImplementedException();
        }

        public Task LoadFromSourceAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> LookupUsersByBirthdayAsync(DateTime birthdayDate, string serverId = null)
        {
            throw new NotImplementedException();
        }

        public Task SaveToSourceAsync()
        {
            throw new NotImplementedException();
        }
    }
}
