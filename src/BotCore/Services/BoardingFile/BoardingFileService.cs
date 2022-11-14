using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        //public async Task<BoardingFiles> GetOrAddAsync(bool isActive, string message, int guildId)
        //{

        //    var boardingMessage = await base.GetAsync(a => a.IsActive);
        //    if (boardingMessage is not null)
        //    {
        //        boardingMessage = await base.UpdateAsync(boardingMessage, x =>
        //        {
        //            x.IsActive = true;
        //            x.Message = message;
        //        });
        //        return boardingMessage;
        //    }


        //    var newMessage = new BoardingMessage()
        //    {
        //        IsActive = isActive,
        //        Message = message,
        //        GuildId = guildId
        //    };
        //    var result = await base.InsertAsync(newMessage);
        //    return result;

        //}


        public async Task<IEnumerable<BoardingFiles>> CheckIfBoardingFilesExist(int boardingMessageId)
        {
           return await base.GetManyAsync(a => a.BoardingMessageId == boardingMessageId);
        }

    }
}
