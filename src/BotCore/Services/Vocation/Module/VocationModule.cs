using BotCore.Entities;
using BotCore.Services.VocationForms;
using Discord;
using Discord.Interactions;
using Domain.Shared.Entities.Bot;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Services.VocationModule
{
    [Group("vocation", "all information about vocation")]
    public class VocationModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public UserService _usersService { get; set; }
        public VocationService vocationService { get; set; }

        [SlashCommand("list", "show all vocation request you did in the past")]
        [ComponentInteraction("vocation-list:*", true)]
        public async Task list(int page = 0)
        {
            await DeferAsync();

            var vocationlist = await vocationService.GetManyAsync(x => x.UserRequestId == Context.BotUser.Id);

            if (vocationlist != null && vocationlist.Count() != 0)
            {
                string times = "";
                string isConfirmed = "";
                string confirmed = "Confirmed";
                string rejected = "Rejected";
                foreach (var vocation in vocationlist)
                {
                    times += $"<t:{((DateTimeOffset)vocation.StartDate).ToUnixTimeSeconds()}:R> to <t:{((DateTimeOffset)vocation.EndDate).ToUnixTimeSeconds()}:R>\n";
                    isConfirmed += (vocation.IsAccept ? confirmed : rejected);
                }

                var embed = new EmbedBuilder()
                    .AddField("IsConfirmed", isConfirmed, true)
                    .AddField("For date", times, true)
                    .Build();

                var components = new ComponentBuilder()
                    .WithButton("Previous", $"vocation-list:{page - 1}", ButtonStyle.Primary, disabled: page > 0)
                    .WithButton("Next", $"vocation-list:{page + 1}", ButtonStyle.Primary, disabled: vocationlist.Count() > 30)
                    .Build();

                await FollowupAsync(embed: embed, components: components);

                return;
            }
            await FollowupAsync("You didn't request for a day off yet!.");
        }



        [SlashCommand("request", "request for a day off")]
        public async Task request()
        {
            await RespondWithModalAsync<VocationForm>($"vocation-request");
            
        }

        [ModalInteraction("vocation-request", true)]
        public async Task set_modal(VocationForm form)
        {
            await DeferAsync();
            
           

            //var user = _usersService.UpdateAsync(Context.BotUser, x =>
            //{
            //    x.Birthday = dateTime;
            //    x.Username = Context.User.Username;
            //    x.GuildId = Context.BotGuild.Id;
            //});

            //await FollowupAsync($"Done, your brithday added, <t:{((DateTimeOffset)dateTime).ToUnixTimeSeconds()}:D>", ephemeral: true);
           
        }
    }
}
