using BotCore.Entities;
using BotCore.Extenstions;
using BotCore.Services;
using BotCore.Utilities;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Interactions.Modules.Admin
{
    [Group("admin", "admin commands")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.ManageRoles)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    [RequireContext(ContextType.Guild)]
    [EnabledInDm(false)]
    public class AdminModule : InteractionModuleBase<CustomSocketInteractionContext>
    {     
        // It is all about how the Admin can manage Roles in discord
        [Group("roles", "roles commands")]
        public class RolesModule : InteractionModuleBase<CustomSocketInteractionContext>
        {
            public BotConfiguration _config { get; set; }
            public ChannelService _channelService { get; set; }


            // the command that admin can run to check if there are some people without desired (Verified ) Role
            [SlashCommand("sync", "sync roles")]
            [RequireGuildSetup(RequireGuildSetup.GuildSetupType.RoleId)]
            public async Task syncRoles()
            {
                await DeferAsync();

                var users = await GuildRolesManagment.GetUsersWithoutAnyRoleAsync(Context.Guild, Context.BotGuild.VerifiedRoleId, Context.BotGuild.UnVerifiedRoleId);
                var usersCount = users.Count();

                var embed = new EmbedBuilder()
                {
                    Title = "Please Wait",
                    Description = $"Syncing the roles for **{usersCount}** users, it may takes time about **{usersCount * 105 / 1000} seconds**.",
                    Color = 16761607,
                    ThumbnailUrl = "https://cdn.discordapp.com/attachments/1022106350219698186/1022529862960947200/4.gif",
                }.Build();

                var msg = await FollowupAsync(embed: embed);

                await GuildRolesManagment.SyncMemberRolesAsync(Context.Guild, users, Context.BotGuild.UnVerifiedRoleId);

                await msg.ModifyAsync(x =>
                {
                    var result = new EmbedBuilder()
                    {
                        Title = "The operation was completed successfully.",
                        Description = $"The operation was completed successfully, roles synced for **{usersCount}** users.",
                        ThumbnailUrl = "https://img.icons8.com/fluency/200/good-quality.png",
                        Color = Color.Green
                    }.Build();
                    x.Embed = result;
                    x.Content = Context.User.ToUserMention();
                });


                // Log the message
                var message = $"All the roles synced by admin";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

            }

            [SlashCommand("add", "add role to user")]
            public async Task add(IGuildUser user, IRole role)
            {
                await DeferAsync();
                if (user.RoleIds.Contains(role.Id))
                {
                    await FollowupAsync("the user have this role already.");
                    return;
                }
                await user.AddRoleAsync(role);
                await FollowupAsync("the role added to the user succesfuly.");

                // Log the message
                var message = $"Role {role.Name} added to {user.Username}";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

            }


            // Remove roles from users
            [SlashCommand("remove", "remove role to user")]
            public async Task remove(IGuildUser user, [Autocomplete()] string role)
            {
                await DeferAsync();

                var roleId = ulong.Parse(role);

                if (!user.RoleIds.Contains(roleId))
                {
                    await FollowupAsync("the user haven't this role.");
                    return;
                }

                await user.RemoveRoleAsync(roleId);
                await FollowupAsync("the role removed from the user succesfuly.");

                // Log the message
                var message = $"Role {roleId} removed from {user.Username}";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

            }


            // AutoComplate the Roles which user already has
            [AutocompleteCommand("role", "remove")]
            public async Task user_roles()
            {
                var socket = Context.Interaction as SocketAutocompleteInteraction;
                var results = new List<AutocompleteResult>();

                var targetUser = socket.Data.Options.FirstOrDefault(a => a.Name == "user");
                if (targetUser != null && targetUser.Value != null)
                {
                    bool parseResult = ulong.TryParse(targetUser.Value.ToString(), out ulong userId);
                    if (parseResult is true)
                    {
                        var user = await Context.Guild.GetUserAsync(userId);
                        foreach (var roleId in user.RoleIds)
                        {
                            var role = Context.Guild.GetRole(roleId);
                            if (role.IsManaged || role.Name == "@everyone") continue;

                            var match = socket.Data.Current.Value.ToString();
                            if (!role.Name.ToLower().Contains(match.ToLower()))
                                continue;

                            var result = new AutocompleteResult()
                            {
                                Name = role.Name,
                                Value = role.Id.ToString()
                            };
                            results.Add(result);
                        }
                    }
                }
                await socket.RespondAsync(results);
            }
        }

    }




}
