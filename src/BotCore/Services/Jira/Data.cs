using Domain.Shared.Entities.Bot;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Uow;

namespace BotCore.Services.Jira
{
    public class Data
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Data(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<BotUser> GetBotUserWithJiraAccount(string JiraAccountId)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<BotUser>>();

                using (var uow = unitOfWorkManager.Begin())
                {
                    var targetUser = await _repository.FindAsync(x => x.JiraAccountID == JiraAccountId);
                    return targetUser;
                }

            }

        }
    }
}
