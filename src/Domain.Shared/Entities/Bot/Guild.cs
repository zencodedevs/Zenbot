using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;

namespace Zenbot.Domain.Shared.Entities.Bot
{
    public class Guild : Entity
    {
        public Guild() : base()
        {
            Channels = new List<GuildChannel>();
        }
        public ulong GuildId { get; set; }
        public string BotPrefix { get; set; }

        public bool IsMainServer { get; set; }

        public string AuthenticationPassword { get; set; }
        public string GreetingFilePath { get; set; }
        public ulong VerifiedRoleId { get; set; }
        public ulong UnVerifiedRoleId { get; set; }
        public ulong HrRoleId { get; set; }

        public string GreetingMessage { get; set; }
        public virtual ICollection<GuildChannel> Channels { get; set; }
    }
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
