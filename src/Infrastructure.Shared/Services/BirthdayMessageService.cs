using Microsoft.EntityFrameworkCore;
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
    public class BirthdayMessageService : IBirthdayMessageService
    {
        private readonly IEntityFrameworkRepository<BirthdayMessage> _repository;
        private readonly IBotUserGuildService _userGuildService;
        public BirthdayMessageService(IEntityFrameworkRepository<BirthdayMessage> repository, IBotUserGuildService userGuildService)
        {
            _repository = repository;
            _userGuildService = userGuildService;
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
                if (string.IsNullOrEmpty(message.Message))
                {
                    bMessage.Message = "Happy Birthday dear {username} We're all happy to have you here and congratulate your birthday together! 😍 \n **Have a very nice day**";
                }
                else bMessage.Message = message.Message;

                bMessage.Message = message.Message;
                bMessage.IsActive = message.IsActive;
                await _repository.UpdateAsync(bMessage);
                await _repository.SaveChangesAsync(true);
                return true;
            }
            return false;
        }
    }
}
