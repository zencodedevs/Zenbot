using Discord.Commands;
using Discord.WebSocket;
using Domain.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zen.Uow;

namespace Zenbot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ITest _test;

        public General(ITest test)
        {
            _test = test;
        }

            [Command("info")]
            public async Task info(SocketGuildUser socketGuildUser = null)
            {

                if (socketGuildUser == null)
                {
                    socketGuildUser = Context.User as SocketGuildUser;
                }

            using (var uow = _unitOfWorkManager.Begin())
            {
                await _test.CreateNewCity();

                await uow.CompleteAsync();
            }

            await ReplyAsync($"ID: {socketGuildUser.Id}\n" +
                    $"Name: {socketGuildUser.Username}#{socketGuildUser.Discriminator}\n" +
                    $"Created at: {socketGuildUser.CreatedAt}");
            }

        
    }
}