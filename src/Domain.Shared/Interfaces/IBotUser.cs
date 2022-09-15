using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared.Interfaces
{
    public interface IBotUser
    {

        Task CreateNewBotUser(string username, string userMail, string userId, byte month, byte day);
    }
}
