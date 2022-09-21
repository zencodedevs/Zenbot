using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot
{
    public class GuildRolesManagment
    {
        public static async Task SyncMemberRoles(IGuild guild, ulong verifiedRoleId, ulong unVerifiedRoleId)
        {
            var users = await (guild as SocketGuild).GetUsersAsync().FlattenAsync();
            var targetUsers = users.Where(a => !a.RoleIds.Contains(verifiedRoleId) && !a.RoleIds.Contains(unVerifiedRoleId));

            foreach (var u in targetUsers)
            {
                try
                {
                    await u.AddRoleAsync(unVerifiedRoleId);
                }
                catch
                {

                }
                finally
                {
                    await Task.Delay(100);
                }
            }

        }
    }
}
