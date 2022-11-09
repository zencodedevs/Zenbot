using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;

namespace Zenbot.Domain.Shared.Entities.Bot
{
    public class BotUserGuild : Entity
    {
        public int BotUserId { get; set; }
        [ForeignKey("BotUserId")]
        public virtual BotUser BotUser {get;set;}

        public int GuildId { get; set; }
        [ForeignKey("GuildId")]
        public virtual Guild Guild { get; set; }
        public bool IsAdmin { get; set; }
    }
}
