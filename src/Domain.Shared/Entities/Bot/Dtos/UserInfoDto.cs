using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Domain.Shared.Entities.Bot.Dtos
{
    public class UserInfoDto
    {
        public string Username { get; set; }
        public string GuildName { get; set; }
        public ulong UserId { get; set; }
        public ulong GuildId { get; set; }
    }
}
