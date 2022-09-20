using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;
using Zen.Domain.Events;

namespace Domain.Shared.Entities.Zenbot
{
    public class BotUser : Entity, IHasDomainEvent
    {
       
        public ulong UserId { get; set; }
        public string UserMail { get; set; }
        public string Username { get; set; }
        public string JiraAccountID { get; set; }
        public DateTime Birthday { get; set; } = DateTime.MinValue;
        public DateTime NextNotifyTIme { get; set; } = DateTime.MinValue;

        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; }

        public BotUser Create(string username, string userMail, ulong userId, DateTime birthday, DateTime nextNotifyTime)
        {
            Birthday = birthday;
            NextNotifyTIme = nextNotifyTime;
            UserId = userId;
            UserMail = userMail;
            Username = username;
            return this;
        }



       
    }
}
