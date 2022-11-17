using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;
using Zen.Domain.Events;
using Zen.Domain.Values;
using Zenbot.Domain.Shared.Entities.Bot;

namespace Domain.Shared.Entities.Bot
{
    public class BotUser : Entity, IHasDomainEvent 
    {
        public ulong DiscordId { get; set; }
        public bool IsSupervisor { get; set; }
        public string UserMail { get; set; }
        public string Username { get; set; }
        public string JiraAccountID { get; set; }
        public string BitBucketAccountId { get; set; }
        public bool IsEnableIntegration { get; set; } = true;
        public DateTime Birthday { get; set; } = DateTime.MinValue;

      


        [NotMapped]
        public virtual ICollection<Vocation> Vocations { get; set; }
        public virtual ICollection<BotUserGuild> BotUserGuilds { get; set; }




        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; }

        public BotUser Create(string username, string userMail, ulong userId, DateTime birthday)
        {
            Birthday = birthday;
            DiscordId = userId;
            UserMail = userMail;
            Username = username;
            return this;
        }
    }

  
    
}
