using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Modules.Birthday
{
    public class help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpAsync()
        {

            var User = Context.User as SocketGuildUser;
            //var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Captain");
            //if (!User.Roles.Contains(role) && User.Username != "Kade") return;

            var builder = new EmbedBuilder()
                .WithTitle("Here are the commands for using Birthday Bot\n")
                .WithDescription("All bot commands use the bday! prefix.")
                .WithColor(new Color(0xA3A1D7))
                .WithFooter(footer => {
                    footer
                        .WithText("Birthday Bot - Created by Zencode - copy right: 2022 - Version 1.0.0");
                })
                .AddField("**bday!add** @User Month Day", "Use #s only not month names. If the user doesn't exist, it will add them. If they do, it will update their birthday.")
                .AddField("**bday!list**", "This will respond with a list of everyone and their birthdays currently known.")
                .AddField("**bday!forceAnnounce**", "This is a fail safe in case the bot doesn't announce any birthdays. (Which is expected if there actually aren't any birthdays, but you get the idea.)");
            await ReplyAsync("", false, builder.Build());
        }
    }
}
