using Microsoft.EntityFrameworkCore;
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
    public class BirthdayMessageService : IBirthdayMessageService
    {
        private readonly IEntityFrameworkRepository<BirthdayMessage> _repository;
        public BirthdayMessageService(IEntityFrameworkRepository<BirthdayMessage> repository)
        {
            _repository = repository;
        }
        public async Task<BirthdayMessage> GetBirthdayMessagesByGuildId(int guildId)
        {
            if (guildId > 0)
            {
                return await _repository.FindAsync(x => x.GuildId == guildId);
            }
            return null;
        }

        public async Task<bool> UpdateBirthdayMessage(BirthdayMessageDto message)
        {
            var bMessage = await _repository.FindAsync(x => x.Id == message.Id);
            if (bMessage != null)
            {
                bMessage.Message = message.Message;
                bMessage.IsActive = message.IsActive;
                await _repository.UpdateAsync(bMessage);
                await _repository.SaveChangesAsync(true);
                return true;
            }
            else if(bMessage == null)            {
                var newBMessage = new BirthdayMessage
                {
                    IsActive = message.IsActive,
                    GuildId = message.GuildId,
                    Message = message.Message
                };
                if (string.IsNullOrEmpty(message.Message))
                {
                    newBMessage.Message = StaticData.BirthdayDefaultMessage;
                }
                else { newBMessage.Message = message.Message; }
                await _repository.InsertAsync(newBMessage);
                await _repository.SaveChangesAsync(true);
                return true;
            };
            return false;
        }

    }
}

