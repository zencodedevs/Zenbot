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
        public ChannelService _channelService { get; set; }


        [SlashCommand("invite", "invite users to join scrinio")]
        public async Task invite(string email)
        {
            await DeferAsync();

            var scrinio = new ScrinIO
            {
                Email = email,
                Token = Context.BotGuild.ScrinIOToken
            };

            var result = await _scrinIOService.InviteUser(scrinio);

            await FollowupAsync(result);


            // Log the message
            var message = $"User invited to join the scrin io";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);
        }

    }
}
