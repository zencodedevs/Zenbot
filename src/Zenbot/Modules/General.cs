using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Domain.Shared.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Interfaces;
using Zen.Uow;
using ZenAchitecture.Domain.Shared.Entities.Geography;

namespace Zenbot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public General(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        [Command("info")]
        public async Task info(SocketGuildUser socketGuildUser = null)
        {


            using (var scope = _scopeFactory.CreateScope())
            {
                var unitOfWorkManager = scope.ServiceProvider.GetRequiredService<IUnitOfWorkManager>();
                var _repository = scope.ServiceProvider.GetRequiredService<IEntityFrameworkRepository<City>>();
                using (var uow = unitOfWorkManager.Begin())
                {
                    var city = new City().Create("zen bot city");
                    await _repository.InsertAsync(city);
                    await _repository.SaveChangesAsync(true);
                }
            }


            if (socketGuildUser == null)
            {
                socketGuildUser = Context.User as SocketGuildUser;
            }

           


            await ReplyAsync($"ID: {socketGuildUser.Id}\n" +
                    $"Name: {socketGuildUser.Username}#{socketGuildUser.Discriminator}\n" +
                    $"Created at: {socketGuildUser.CreatedAt}");
        }

    }
}