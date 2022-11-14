using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenbot.Domain.Shared.Common;
using Zenbot.Domain.Shared.Entities.Bot;

namespace BotCore.Services
{
    public class BoardingFileService : EntityBaseService<BoardingFiles>
    {
        private readonly DiscordSocketClient _discord;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _services;


        public BoardingFileService(IServiceProvider services, IServiceScopeFactory scopeFactory) : base(services, scopeFactory)
        {
            _services = services;
            _scopeFactory = scopeFactory;
            _discord = _services.GetRequiredService<DiscordSocketClient>();
        }


       public async Task<BoardingFiles> AddBoardingFilesAsync(int brMessageId,string file)
        {
            var brfiles = await base.GetManyAsync(x => x.BoardingMessageId == brMessageId);
            if (brfiles != null)
            {
                foreach (var item in brfiles)
                {
                    UploadFiles.DeleteImg("wwwroot/"+item.FilePath);
                }
            }
            await base.DeleteManyAsync(brfiles);

            var data = new BoardingFiles
            {
                BoardingMessageId = brMessageId,
                FilePath = file
            };
            return await base.InsertAsync(data);
        }


        public async Task<IEnumerable<BoardingFiles>> CheckIfBoardingFilesExist(int boardingMessageId)
        {
           return await base.GetManyAsync(a => a.BoardingMessageId == boardingMessageId);
        }

    }
}
