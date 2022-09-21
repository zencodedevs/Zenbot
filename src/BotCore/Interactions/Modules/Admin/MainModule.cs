using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Interactions.Modules.Admin
{
    [Group("admin", "admin commands")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.ManageRoles)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public class MainModule : InteractionModuleBase<CustomSocketInteractionContext>
    {

        [Group("setup", "setup guild settings")]
        public class SetupModule : InteractionModuleBase<CustomSocketInteractionContext>
        {
            public BotConfiguration botConfiguration { get; set; }

            [SlashCommand("sync-roles", "sync roles")]
            public async Task syncRoles()
            {
                await DeferAsync();
                var msg = await FollowupAsync("Syncing the roles.");

                await GuildRolesManagment.SyncMemberRoles(Context.Guild, botConfiguration.Roles.VarifiedId, botConfiguration.Roles.UnVarifiedId);

                await msg.ModifyAsync(x =>
                {
                    x.Content = "Done, users roles are synced.";
                });
            }
            [SlashCommand("authentication", "setup authentication channel")]
            public async Task authentication(ITextChannel channel)
            {
                await DeferAsync();

                var embed = new EmbedBuilder()
                {
                    Title = "Authentication",
                    Description = "Specify your identity, so that the necessary accesses are issued.",
                    ThumbnailUrl = "https://img.icons8.com/fluency/344/identification-documents.png",
                };
                var component = new ComponentBuilder()
                    .WithButton("Authentication", "button-admin-setup-authentication-password", ButtonStyle.Success, new Emoji("🔒"), null, false, 0);

                await channel.SendMessageAsync(embed: embed.Build(), components: component.Build());
            }

        }
    }

    [RequireBotPermission(GuildPermission.ManageRoles)]
    public class SharedModules : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public BotConfiguration botConfiguration { get; set; }
        public EventService EventService { get; set; }

        [ComponentInteraction("button-admin-setup-authentication-password", true)]
        [RequireGuildRole(RequireGuildRole.RoleType.UnVerified)]
        public async Task btnAuthentication()
        {
            await RespondWithModalAsync<AuthenticationForm>("modal-admin-setup-authentication-password");
        }

        [ModalInteraction("modal-admin-setup-authentication-password", true)]
        public async Task modalAuthentication(AuthenticationForm modal)
        {
            await DeferAsync(true);

            if (modal.Password.Equals("1"))
            {
                try
                {
                    await EventService.SendMessageToLoggerChannel($"@everyone Say Welcome To {MentionUtils.MentionUser(Context.User.Id)}");

                    await Context.User.SendFileAsync(@"wwwroot/documents/sample.pdf", "Download the file");
                    
                    await (Context.User as IGuildUser).AddRoleAsync(botConfiguration.Roles.VarifiedId);
                    await (Context.User as IGuildUser).RemoveRoleAsync(botConfiguration.Roles.UnVarifiedId);

                    await FollowupAsync("You are verified now.", ephemeral: true);

                    return;
                }
                catch
                {
                    await FollowupAsync("The bot can't send message in your direct, make sure yoru direct is open.", ephemeral: true);
                    return;
                }
            }

            await FollowupAsync("Your entered password is wrong.", ephemeral: true);
        }
    }
}
