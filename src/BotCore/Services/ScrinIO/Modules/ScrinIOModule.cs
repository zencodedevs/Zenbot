using BotCore.Entities;
using BotCore.Services.ScrinIO;
using Discord;
using Discord.Interactions;
using System.Threading.Tasks;

namespace BotCore.Services.ScrinIO.Modules
{

    // Scrin Io Commands and Sending invitation to desired user

    [Group("scrin", "admin commands")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.ManageRoles)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public class ScrinIOModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public ScrinIOService _scrinIOService { get; set; }


        [SlashCommand("invite", "invite users to join scrinio")]
        public async Task invite(string email)
        {
            await DeferAsync();

            var scrinio = new ScrinEmail
            {
                Email = email
            };

            var result = await _scrinIOService.InviteUser(scrinio);

            await FollowupAsync(result);
        }

    }
}
