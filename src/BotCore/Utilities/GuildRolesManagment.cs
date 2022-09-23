using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.BotCore
{
    public class GuildRolesManagment
    {
        public static async Task<IEnumerable<IGuildUser>> GetUsersWithoutAnyRoleAsync(IGuild guild, ulong verifiedRoleId, ulong unVerifiedRoleId)
        {
            var users = await (guild as SocketGuild).GetUsersAsync().FlattenAsync();
            var targetUsers = users.Where(a => !a.RoleIds.Contains(verifiedRoleId) && !a.RoleIds.Contains(unVerifiedRoleId));
            return targetUsers;
        }
        public static async Task SyncMemberRolesAsync(IEnumerable<IGuildUser> users, ulong unVerifiedRoleId)
        {
            foreach (var u in users)
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
