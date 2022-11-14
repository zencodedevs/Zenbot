using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zenbot.Domain.Shared.Common;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;
using Zenbot.Domain.Shared.Interfaces;

namespace Zenbot.Infrastructure.Shared.Services
{
    public class BoardingFilesServices : IBoardingFiles
    {
        private readonly IEntityFrameworkRepository<BoardingFiles> _repository;
        private readonly IBoardingMessage _boardingMessage;
        public BoardingFilesServices(IEntityFrameworkRepository<BoardingFiles> repository, IBoardingMessage boardingMessage)
        {
            _repository = repository;
            _boardingMessage = boardingMessage;
        }
        public async Task<List<BoardingFiles>> GetBoardingFilesByBoardingMessageId(int messageId)
        {
            return await _repository.GetListAsync(x => x.BoardingMessageId == messageId);
        }

        public async Task<bool> UpdateBoardingFiles(List<IFormFile> file, int messageId, int guildId)
        {
            var msgId = messageId;
            if (!(messageId > 0))
            {
                msgId = await _boardingMessage.CreateMessage(guildId);
            }
            if (file != null && file.Count > 0)
            {
                // Find the previous files and delete all of them
                var boardingFiles = await _repository.GetListAsync(x => x.BoardingMessageId == msgId);
                if (boardingFiles != null)
                {
                    foreach (var item in boardingFiles)
                    {
                        UploadFiles.DeleteImg(item.FilePath);
                    }
                    await _repository.DeleteManyAsync(boardingFiles);
                }

                // Insert new files 
                var fileList = new List<BoardingFiles>();

                foreach (IFormFile item in file)
                {
                    string imgname = UploadFiles.CreateImg(item);
                    if (imgname == "false") return false;
                    var boardingFile = new BoardingFiles
                    {
                        BoardingMessageId = msgId,
                        FilePath = "bot/boardingFile/" + imgname
                    };
                    fileList.Add(boardingFile);
                }
                await _repository.InsertManyAsync(fileList);
                await _repository.SaveChangesAsync(true);
                return true;
            }
            return false;

        }
    }
}
