using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;

namespace Zenbot.Domain.Shared.Entities.Bot
{
    public class GuildChannel : Entity
    {
        public ulong ChannelId { get; set; }
        public GuildChannelType Type { get; set; }

        public int GuildId { get; set; }
        public Guild Guild { get; set; }
    }

    public enum GuildChannelType
    {
        None,
        Authentication,
        Logger
    }
}
