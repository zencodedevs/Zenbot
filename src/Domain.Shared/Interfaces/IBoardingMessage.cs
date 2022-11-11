using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;

namespace Zenbot.Domain.Shared.Interfaces
{
    public interface IBoardingMessage
    {
        Task<BoardingMessage> GetBoardingMessagesByGuildId(int guildId);
        Task<bool> UpdateBoardingMessage(BoardingMessageDto message);
    }
}
