using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;

namespace Zenbot.Domain.Shared.Entities.Bot
{
    public class BirthdayMessage : Entity
    {
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public int GuildId { get; set; }
        public Guild Guild { get; set; }
    }
}
