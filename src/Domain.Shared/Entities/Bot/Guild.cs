using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public string GuildName { get; set; }
        public bool IsMainServer { get; set; }
        public string ScrinIOToken { get; set; }
        public string AuthenticationPassword { get; set; }
        public string GreetingFilePath { get; set; }
        public string GSuiteAuth { get; set; }
        public ulong VerifiedRoleId { get; set; }
        public ulong UnVerifiedRoleId { get; set; }
        public ulong HrRoleId { get; set; }

        public virtual ICollection<GuildChannel> Channels { get; set; }
        public virtual ICollection<BirthdayMessage> BirthdayMessages { get; set; }
        public virtual ICollection<WelcomeMessage> WelcomeMessages { get; set; }
        public virtual ICollection<BotUserGuild> BotUserGuilds { get; set; }
    }
 
  
}
