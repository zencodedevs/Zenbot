using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot
{
    public class UsersService
    {
        ConcurrentBag<BUser> users = new ConcurrentBag<BUser>();
        public List<BUser> GetUsersBirthday()
        {
            return users.Where(a => a.NoticeBrithday).ToList();
        }
        public BUser GetUser(ulong Id)
        {
            var user = users.FirstOrDefault(a => a.Id == Id);
            if (user is null)
            {
                user = new BUser()
                {
                    Id = Id,
                };
                users.Add(user);
            }
            return user;
        }
    }
}