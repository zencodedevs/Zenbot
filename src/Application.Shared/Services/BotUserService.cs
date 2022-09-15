using Domain.Shared.Entities.Zenbot;
using Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using ZenAchitecture.Domain.Shared.Entities.Geography;

namespace Application.Shared.Services
{
    public class BotUserService : IBotUser
    {
        private readonly IEntityFrameworkRepository<BotUser> _repository;
        public BotUserService(IEntityFrameworkRepository<BotUser> repository)
        {
            _repository = repository;
        }

        public async Task CreateNewBotUser(string username, string userMail, string userId, byte month, byte day)
        {
            try
            {
                var botUser = new BotUser().Create(username, userMail, userId, month, day);
               
                await _repository.InsertAsync(botUser);
                await _repository.SaveChangesAsync(true);
            }
            catch (Exception ex)
            {
                var d = ex;
            }
        }

       
    }
}
