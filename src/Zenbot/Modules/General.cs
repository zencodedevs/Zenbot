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
using Zenbot.Services;

namespace Zenbot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        
        private CommandHandler _handler;

        // constructor injection is also a valid way to access the dependecies
        public General(CommandHandler handler)
        {
            _handler = handler;
        }
        [Command("info")]
        public async Task info(SocketGuildUser socketGuildUser = null)
        {

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