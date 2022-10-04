
using Discord;
using Discord.Interactions;
using System.Threading.Tasks;
using Zenbot.BotCore;
using Zenbot.BotCore.Interactions;

namespace BotCore.Interactions.Modules.Scrin.io
{

    // Scrin Io Commands and Sending invitation to desired user

    [Group("scrin", "admin commands")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.ManageRoles)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public class ScrinIOModule : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public ScrinIOService ScrinIOService { get; set; }


        [SlashCommand("invite", "invite users to join scrinio")]
        public async Task invite(string email)
        {
            await DeferAsync();

            var scrinio = new ScrinEmail
            {
                Email = email
            };

            var result = await ScrinIOService.InviteUser(scrinio);
           
                await FollowupAsync(result);
           
        }

    }
}
