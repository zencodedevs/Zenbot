using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;

namespace Zenbot.Domain.Shared.Interfaces
{
    public interface IBoardingFiles
    {
        Task<List<BoardingFiles>> GetBoardingFilesByBoardingMessageId(int messageId);
        Task<bool> UpdateBoardingFiles(List<IFormFile> file, int messageId, int guildId);
    }
}
