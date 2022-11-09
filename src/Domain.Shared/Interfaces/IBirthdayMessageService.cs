using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;

namespace Zenbot.Domain.Shared.Interfaces
{
    public interface IBirthdayMessageService
    {
        Task<List<BirthdayMessage>> GetBirthdayMessagesByGuildId(ulong userId);
        Task<bool> UpdateBirthdayMessage(BirthdayMessageDto messageDto);
    }
}
