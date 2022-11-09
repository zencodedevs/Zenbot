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
        public async Task<List<BirthdayMessage>> GetBirthdayMessagesByGuildId(ulong userId)
        {
            if (!string.IsNullOrEmpty(userId.ToString()))
            {
                var guilds = await _userGuildService.GetAllGuildsByUserId(userId);
                var messages = new List<BirthdayMessage>();
                foreach (var guild in guilds)
                {
                    var query = await _repository.GetQueryableAsync(x => x.Guild);
                    var message = await query.Where(x => x.GuildId == guild.Id).FirstOrDefaultAsync();
                    messages.Add(message);
                }
                return messages;
            }
            return null;
        }

        public async Task<bool> UpdateBirthdayMessage(BirthdayMessageDto messageDto)
        {
            var message = await _repository.FindAsync(x => x.Id == messageDto.Id);
            if (message != null)
            {
                message.Message = message.Message;
                message.IsActive = messageDto.IsActive;
                await _repository.UpdateAsync(message);
                await _repository.SaveChangesAsync(true);
                return true;
            }
            return false;
        }
    }
}
