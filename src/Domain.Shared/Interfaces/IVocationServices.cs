using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;

namespace Zenbot.Domain.Shared.Interfaces
{
    public interface IVocationServices
    {
        Task<List<Vocation>> GetVocationListByGuildId(int guildId);
        //Task<bool> UpdateBirthdayMessage(BirthdayMessageDto message);
    }
}
