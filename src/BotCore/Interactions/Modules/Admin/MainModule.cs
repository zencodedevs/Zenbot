using BotCore;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.BotCore.Interactions.Modules.Admin
{
    [Group("admin", "admin commands")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(GuildPermission.ManageRoles)]
    [DefaultMemberPermissions(GuildPermission.Administrator)]
    public class MainModule : InteractionModuleBase<CustomSocketInteractionContext>
    {

        // It is all about how the Admin can manage Roles in discord

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


        // the command that admin can run to check if there are some people without desired (Verified ) Role
        [Group("setup", "setup guild settings")]
        public class SetupModule : InteractionModuleBase<CustomSocketInteractionContext>
        {
            public BotConfiguration botConfiguration { get; set; }

            [SlashCommand("sync-roles", "sync roles")]
            public async Task syncRoles()
            {
                await DeferAsync();

                var users = await GuildRolesManagment.GetUsersWithoutAnyRoleAsync(Context.Guild, botConfiguration.Roles.VarifiedId, botConfiguration.Roles.UnVarifiedId);
                var usersCount = users.Count();

                var embed = new EmbedBuilder()
                {
                    Title = "Please Wait",
                    Description = $"Syncing the roles for **{usersCount}** users, it may takes time about **{(usersCount * 105) / 1000} seconds**.",
                    Color = 16761607,
                    ThumbnailUrl = "https://cdn.discordapp.com/attachments/1022106350219698186/1022529862960947200/4.gif",
                }.Build();

                var msg = await FollowupAsync(embed: embed);

                await GuildRolesManagment.SyncMemberRolesAsync(users, botConfiguration.Roles.UnVarifiedId);

                await msg.ModifyAsync(x =>
                {
                    var embed1 = new EmbedBuilder()
                    {
                        Title = "The operation was completed successfully.",
                        Description = $"The operation was completed successfully, roles synced for **{usersCount}** users.",
                        ThumbnailUrl = "https://img.icons8.com/fluency/200/good-quality.png",
                        Color = Color.Green
                    }.Build();
                    x.Embed = embed1;
                    x.Content = Context.User.Id.ToUserMention();
                });
            }
            [SlashCommand("authentication", "setup authentication channel")]
            public async Task authentication(ITextChannel channel)
            {
                await DeferAsync(true);

                var embed = new EmbedBuilder()
                {
                    Title = "Authentication",
                    Description = "Specify your identity, so that the necessary accesses are issued.",
                    ThumbnailUrl = "https://img.icons8.com/fluency/344/identification-documents.png",
                };
                var component = new ComponentBuilder()
                    .WithButton("Authentication", "button-admin-setup-authentication-password", ButtonStyle.Success, new Emoji("🔒"), null, false, 0);

                await channel.SendMessageAsync(embed: embed.Build(), components: component.Build());

                await FollowupAsync("The channel updated, make sure the channel is private and unwriteable.", ephemeral: true);
            }

        }
    }



    // Here is the part that New users should enter the server password so the can be part of this server
    [RequireBotPermission(GuildPermission.ManageRoles)]
    public class SharedModules : InteractionModuleBase<CustomSocketInteractionContext>
    {
        public BotConfiguration botConfiguration { get; set; }
        public EventService EventService { get; set; }

        [ComponentInteraction("button-admin-setup-authentication-password", true)]
        [RequireGuildRole(RequireGuildRole.RoleType.UnVerified)]
        [RateLimit(10, 1, RateLimit.RateLimitType.User, RateLimit.RateLimitBaseType.BaseOnMessageComponentCustomId)]
        public async Task btnAuthentication()
        {
            await RespondWithModalAsync<AuthenticationForm>("modal-admin-setup-authentication-password");
        }


        // The Modal which unverified user should enter their password
        [ModalInteraction("modal-admin-setup-authentication-password", true)]
        public async Task modalAuthentication(AuthenticationForm modal)
        {
            await DeferAsync(true);

            if (!modal.Password.Equals("1"))
            {
                var emebd = new EmbedBuilder()
                {
                    Title = "Wrong Password !",
                    Description = "Your entered password is wrong.",
                    ThumbnailUrl = "https://img.icons8.com/fluency/200/restriction-shield.png",
                    Color = Color.Red
                }.Build();

                await FollowupAsync(embed: emebd, ephemeral: true);
                return;
            }

            var embed = new EmbedBuilder()
            {
                Title = "Be Carefull !",
                Description = "Make sure your direct (DM) is open to send a message then click on the **confirm** button.",
                ThumbnailUrl = "https://img.icons8.com/fluency/344/important-mail.png",
                Color = 506623
            }.Build();

            var component = new ComponentBuilder()
                .WithButton("Confirm", "button-admin-setup-authentication-password-confirm", ButtonStyle.Success, new Emoji("⚒"), null, false, 0);

            await FollowupAsync(embed: embed, components: component.Build(), ephemeral: true);
        }


        /// <summary>
        /// Errro handling if the user's direct is not open, first they should open thier direct so the bot can send them message and 
        /// on boarding file
        /// </summary>
        /// <returns></returns>
        [ComponentInteraction("button-admin-setup-authentication-password-confirm")]
        [RequireGuildRole(RequireGuildRole.RoleType.UnVerified)]
        [RateLimit(10, 1, RateLimit.RateLimitType.User, RateLimit.RateLimitBaseType.BaseOnMessageComponentCustomId)]
        public async Task confirm()
        {
            await DeferAsync();

            if (!System.IO.File.Exists(botConfiguration.StaticFiles.GreetingFile))
            {
                await FollowupAsync("File not found.", ephemeral: true);
                return;
            }

            try
            {
                await Context.User.SendFileAsync(
                    filePath: botConfiguration.StaticFiles.GreetingFile,
                    text: string.Format(botConfiguration.Text.GreetingMessage, Context.User.Username));
            }
            catch
            {
                await FollowupAsync("The bot can't send message in your direct, make sure yoru direct is open.", ephemeral: true);
                return;
            }

            await (Context.User as IGuildUser).AddRoleAsync(botConfiguration.Roles.VarifiedId);

            var embed = new EmbedBuilder()
            {
                Title = $"{Context.User.Username} Joined",
                Description = $"@everyone Say Welcome To <@{Context.User.Id}>.",
                ThumbnailUrl = "https://img.icons8.com/fluency/200/confetti.png",
                Author = new EmbedAuthorBuilder()
                {
                    Name = Context.User.Username,
                    IconUrl = Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl()
                }
            }.Build();

            await EventService.SendMessageToLoggerChannelAsync(Context.User.Id.ToUserMention(), embed: embed);

            await (Context.User as IGuildUser).RemoveRoleAsync(botConfiguration.Roles.UnVarifiedId);
        }
    }
}
