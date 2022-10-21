using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Utilities
{
    public class GuildRolesManagment
    {

        /// <summary>
        /// Server Role for Users and check for authetiaction the new users
        /// </summary>
        /// <param name="guild"></param>
        /// <param name="verifiedRoleId"></param>
        /// <param name="unVerifiedRoleId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<IGuildUser>> GetUsersWithoutAnyRoleAsync(IGuild guild, ulong verifiedRoleId, ulong unVerifiedRoleId)
        {
            var users = await (guild as SocketGuild).GetUsersAsync().FlattenAsync();
            var targetUsers = users.Where(a => !a.RoleIds.Contains(verifiedRoleId) && !a.RoleIds.Contains(unVerifiedRoleId));
            return targetUsers;
        }


        public static async Task<ICollection<IGuildUser>> SyncMemberRolesAsync(IGuild guild, IEnumerable<IGuildUser> users, ulong unVerifiedRoleId)
        {
            if (!guild.Roles.Select(a => a.Id).Contains(unVerifiedRoleId))
                return null;

            List<IGuildUser> error = new List<IGuildUser>();

            foreach (var u in users)
            {
                try
                {
                    await u.AddRoleAsync(unVerifiedRoleId);
                }
                catch
                {
                    error.Add(u);
                }
                finally
                {
                    await Task.Delay(100);
                }
            }
            return error;
        }
    }
}
