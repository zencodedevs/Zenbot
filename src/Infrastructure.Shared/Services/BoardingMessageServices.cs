using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zenbot.Domain.Shared.Entities.Bot;
using Zenbot.Domain.Shared.Entities.Bot.Dtos;
using Zenbot.Domain.Shared.Interfaces;

namespace Zenbot.Infrastructure.Shared.Services
{
    public class BoardingMessageServices : IBoardingMessage
    {
        private readonly IEntityFrameworkRepository<BoardingMessage> _repository;
        public BoardingMessageServices(IEntityFrameworkRepository<BoardingMessage> repository)
        {
            _repository = repository;
        }
        public async Task<BoardingMessage> GetBoardingMessagesByGuildId(int guildId)
        {
            if (guildId > 0)
            {
                return await _repository.FindAsync(x => x.GuildId == guildId);
            }
            return null;
        }

        public async Task<bool> UpdateBoardingMessage(BoardingMessageDto message)
        {
            var wMessage = await _repository.FindAsync(x => x.Id == message.Id);
            if (wMessage != null)
            {
                wMessage.Message = message.Message;
                wMessage.IsActive = message.IsActive;
                await _repository.UpdateAsync(wMessage);
                await _repository.SaveChangesAsync(true);
                return true;
            }
            else if (wMessage == null)
            {
                var newWMessage = new BoardingMessage
                {
                    IsActive = message.IsActive,
                    GuildId = message.GuildId,
                    Message = message.Message
                };
                if (string.IsNullOrEmpty(message.Message))
                {
                    newWMessage.Message = "Welcome dear {username} \n Now we're much stronger by having you in our team!\n **Thank you for joining us**";
                }
                else { newWMessage.Message = message.Message; }
                await _repository.InsertAsync(newWMessage);
                await _repository.SaveChangesAsync(true);
                return true;
            };
            return false;
        }
    }
}
