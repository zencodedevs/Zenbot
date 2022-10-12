using BotCore.Entities;
using BotCore.Extenstions;
using BotCore.Interactions.Preconditions;
using BotCore.Services;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Interactions.Modules.Moderators
{
    /// <summary>
    /// Here is all HR commands for Managing the Roles of server
    /// </summary>

    [RequireBotPermission(GuildPermission.ManageRoles)]
    [RequireGuildRole(RequireGuildRole.RoleType.HR)]
    [Group("hr", "hr commands")]
    public class HRModule : InteractionModuleBase<CustomSocketInteractionContext>
    {

        // Users Group which HR sends onboarding file or ther required file to users
        [Group("users", "users commands")]
        public class UsersModule : InteractionModuleBase<CustomSocketInteractionContext>
        {
            public BotConfiguration BotConfiguration { get; set; }
            public GuildService guildService { get; set; }

            [SlashCommand("send-file", "send file to a user")]
            public async Task sendFile(IGuildUser user, bool @private, IAttachment file)
            {
                await DeferAsync(@private);
                
                var welcomeMessage = await guildService.GetWelcomeMessageAsync(Context.BotGuild.Id);// getting data from database

                // Message text and replace the {username} with Discord username
                var wMessage = welcomeMessage.Message.Replace("{username}", $"<@{user.Id}>");

                var embed = new EmbedBuilder()
                {
                    Title = "New File Received",
                    Description =
                    $"**You have new {(file.Ephemeral ? "private " : "")} file from <@{Context.User.Id}>\n\n**" +
                    $"Description: {wMessage}\n\n" +
                    $"Size: ` {file.Size.ToSizeSuffix()} `\n" +
                    $"File Name: ` {file.Filename} `\n" +
                    $"**[Download The File]({file.Url})**",
                    ThumbnailUrl = "https://img.icons8.com/fluency/344/double-down.png",
                    Color = 5814783,
                };

                var component = new ComponentBuilder()
                    .WithButton("Download File", null, ButtonStyle.Link, new Emoji("🌐"), file.Url, false, 0);

                await user.SendMessageAsync(embed: embed.Build(), components: component.Build());

                await FollowupAsync("File sent to the user succesfully.", ephemeral: @private);
            }
        }



        // Managing Role by Commands
        [Group("roles", "roles commands")]
        public class RolesModule : InteractionModuleBase<CustomSocketInteractionContext>
        {

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
            }

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
            }

            [AutocompleteCommand("role", "remove")]
            public async Task user_roles()
            {
                var socket = (Context.Interaction as SocketAutocompleteInteraction);

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
