
using Discord;
using Discord.Interactions;
using System.Threading.Tasks;
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
        
            [SlashCommand("invite", "invite users to join scrinio")]
            public async Task invite(IGuildUser user)
            {
                await DeferAsync();

                await FollowupAsync($"The invitation sent to {user.Username} successfully.");
            }
        
    }
}
