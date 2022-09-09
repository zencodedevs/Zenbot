using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {

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