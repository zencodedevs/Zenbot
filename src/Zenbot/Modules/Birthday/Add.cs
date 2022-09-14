using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Modules.Birthday
{
    internal class Add : ModuleBase<SocketCommandContext>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Add(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        // Uses to add/update birthdays to record. Limited to Captain only.
        [Command("add")]
        public async Task BdayAsync(int Month, int Day)
        {

            var User = Context.User as SocketGuildUser;
          
            // save the birthday date to database
           

            await ReplyAsync("Perfect! I won't forget it!");
        }
    }
}
