using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;

namespace Zenbot.Domain.Shared.Entities.Bot
{
    public class ZenbotServer: Entity
    {
        public ulong ServerID { get; set; }
        public string ServerPassword { get; set; }
        public ulong GeneralChannelId { get; set; }
        public string ServerJoinedMessage { get; set; }
    }
}
