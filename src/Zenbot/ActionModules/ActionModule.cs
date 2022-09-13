
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenbot.ActionAttributes;
using Zenbot.Configuration;
using Zenbot.Data;
using Zenbot.Services;

namespace Zenbot.ActionModules
{
    /*
     * Stores Actions to be executed by Bot on its own
     */
    public class ActionModule : IActionModule
    {
        private readonly IDiscordClient _client;
        private readonly RestService _myRest;
        private readonly IBirthdaysRepository _birthdays;
        private readonly IOptions<BirthdayConfiguration> _birthdayConfig;
        public ActionModule(IOptions<BirthdayConfiguration> birthdayConfig, DiscordSocketClient client, RestService myRest, IBirthdaysRepository birthdays)
        {
            _birthdayConfig = birthdayConfig;
            _client = client;
            _myRest = myRest;
            _birthdays = birthdays;
        }

        /// <summary>
        /// Action for automatic birthday checkup / assignment.
        /// </summary>
        /// <remarks>
        /// Check if any of the birthdays are today.
        /// <br/>For all discovered birthdays:
        /// <br/> - Assign Birthday Role to corresponding user in all applicable servers.
        /// <br/> - Send a congratulatory message to a default channel of each applicable server.
        /// </remarks>
        [RunAtStartup]
        [Timer(Interval.SECOND * 5)]
        public async Task SetBirthdaysActionAsync()
        {
            Console.WriteLine("[SetBirthdaysAction] Execution has began.");

            List<string> todaysBirthdays = new(await _birthdays.LookupUsersByBirthdayAsync(DateTime.Today));

            if (todaysBirthdays.Count() == 0)
            {
                Console.WriteLine("[SetBirthdaysAction] Execution completed. No birthdays detected.");
                return;
            }

            var guilds = (await _client.GetGuildsAsync()).ToList<IGuild>();
            var guildsEditable = guilds as ICollection<IGuild>;
            var roleName = _birthdayConfig.Value.RoleName;
            string roleId;
            SocketTextChannel defaultChannel;

            if (String.IsNullOrEmpty(roleName))
            {
                throw new ArgumentException("Birthday Role name is not defined.");
            }

            if (guilds is null || guilds.Count == 0)
            {
                throw new ArgumentException("Zenbot is not in any guilds.");
            }

            foreach (string userId in todaysBirthdays)
            {
                foreach (var guild in guilds)
                {
                    try
                    {
                        roleId = guild.Roles.First(sp_role => sp_role.Name == roleName).Id.ToString();
                    }
                    catch (Exception e) // If one guild doesn't have correct Birthday Role set up - we still want to congratulate users in other guilds
                    {
                        Console.WriteLine($"[SetBirthdaysAction] {e.Message}");
                        guilds.Remove(guild); // This way we don't check this guild for every user
                        continue;
                    }

                    defaultChannel = (SocketTextChannel)await guild.GetDefaultChannelAsync();
                    if (defaultChannel is null)
                    {
                        Console.WriteLine($"[Guild: {guild.Name}] does not have accessible channels to wish users Happy Birthday.");
                        guilds.Remove(guild); // This way we don't check this guild for every user
                        continue;
                    }

                    try // TODO: Have the bot give it a few tries before giving up
                    {
                        // Add role (by id) to the user (by id) - requires no IRole or IUser objects
                        await _myRest.PutAsync("guilds/" + guild.Id + "/members/" + userId + "/roles/" + roleId, null);
                    }
                    catch (Exception e)
                    {
                        // if PUT failed to execute - birthday assignment can be handled by admin
                        // No need to crash the bot for this
                        Console.WriteLine($"Birthday Role failed to assign for [User Id: {userId}] in [Guild: {guild.Name}].");
                        continue;
                    }

                    // When using @mention by User Id in guild channel, user's Nickname will be substituted in automatically
                    await defaultChannel.SendMessageAsync($"<@{userId}> has a birthday today! Happy Birthday!");
                }
            }

            Console.WriteLine($"[{DateTime.Now.ToString()}] [SetBirthdaysAction] Execution completed. {todaysBirthdays.Count()} birthdays detected.");
        }

    }
}
