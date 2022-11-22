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
using Zenbot.Domain.Shared.Common;

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
        public ChannelService _channelService { get; set; }
        // Select or change the Guild logger channel
        [SlashCommand("logger-channel", "setup logger channel")]
        public async Task logger_channel(ITextChannel channel)
        {
            await DeferAsync();


            await FollowupAsync($"The channel ID you've requested for : `{channel.Id}`");

            // Log the message
            var message = $"Selected `{Context.Channel.Name}` as logger channel";
            await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

        }

        // Users Group which HR sends onboarding file or ther required file to users
        [Group("users", "users commands")]
        public class UsersModule : InteractionModuleBase<CustomSocketInteractionContext>
        {
            public ChannelService _channelService { get; set; }
            public BotConfiguration BotConfiguration { get; set; }
            public GuildService guildService { get; set; }
            public BoardingServices boardingServices { get; set; }
            public UserService userService { get; set; }
            public BotUserGuildServices _botUserGuildServices { get; set; }


            [SlashCommand("send-file", "send file to a user")]
            public async Task sendFile(IGuildUser user, bool @private, IAttachment file)
            {
                await DeferAsync(@private);

                var brMessage = await boardingServices.GetBoardingMessageAsync(Context.BotGuild.Id);// getting data from database

                // Message text and replace the {username} with Discord username
                if (brMessage.IsActive)
                {
                    var bMessage = brMessage.Message.Replace("{username}", $"<@{user.Id}>");
                    var embed = new EmbedBuilder()
                    {
                        Title = "Boarding message from HR",
                        Description =
                        $"**You have new {(file.Ephemeral ? "private " : "")} file from <@{Context.User.Id}>\n\n**" +
                        $"Description: {brMessage}\n\n" +
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

                await FollowupAsync("Boarding message is not Enable, please Enable that by runing `setup-boarding-message` or login to zenbot website");


                // Log the message
                var message = $"`{Context.User.Username}` Sent a on-boarding file {file.Url} to `{user.Username}`";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

            }

            [SlashCommand("add-birthday","register birthday date for a user, type: dd/MM/yyyy")]
            public async Task RegisterUser(IGuildUser botUser, DateTime birthday)
            {
                await DeferAsync();

                // Add Server and discord user into database
                var guild = await guildService.GetOrAddAsync(Context.Guild.Id);
                var user = await userService.GetOrAddAsync(botUser.Id, botUser.Username, birthday);
                
                await _botUserGuildServices.GetOrAddAsync(guild.Id, user.Id, false);

                DateTime bthDay = birthday;
                await FollowupAsync($"Done, brithday added for `{botUser.Username}`, <t:{((DateTimeOffset)bthDay).ToUnixTimeSeconds()}:D>", ephemeral: true);

                // Log the message
                var message = $"`{Context.User.Username}` as HR added birthday for `{botUser.Username}` : Date: `{birthday}`";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

            }


            [SlashCommand("generate-new-password", "setup server password")]
            public async Task password(string password)
            {
                await DeferAsync();
                await Context._guildService.UpdateAsync(Context.BotGuild.Id, x =>
                {
                    x.AuthenticationPassword = password;
                });
                await FollowupAsync($"The password changed to {password} succesfuly.");

                // Log the message
                var message = $"New password generated for server";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

            }



            [SlashCommand("add-integration", "register jiraAccount ID and bitBucket Account ID")]
            public async Task Integration(IGuildUser botUser,string jiraAccountID, string bitBucketAccountID)
            {
                await DeferAsync();

                // Add Server and discord user into database
                var guild = await guildService.GetOrAddAsync(Context.Guild.Id);
                var user = await userService.GetOrAddAsync(botUser.Id, botUser.Username, jiraAccountID, bitBucketAccountID);

                await _botUserGuildServices.GetOrAddAsync(guild.Id, user.Id, false);
                await FollowupAsync($"Done, JiraID : `{jiraAccountID}`, BitBucketID: `{bitBucketAccountID}` for `{botUser.Username}", ephemeral: true);

                // Log the message
                var message = $"`{Context.User.Username}` as HR added birthday for `{botUser.Username}` jira and bitbucket account ID";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

            }
        }



        // Managing Role by Commands
        [Group("roles", "roles commands")]
        public class RolesModule : InteractionModuleBase<CustomSocketInteractionContext>
        {
            public ChannelService _channelService { get; set; }


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
                var message = $"Role `{role.Name}` added to `{user.Username}`";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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

                // Log the message
                var message = $"Role `{roleId}` removed `{user.Username}`";
                await _channelService.loggerEmbedMessage(message, Context.Guild.Name, Context.Guild.Id, Context.User.Username, Context.User.Id);

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
