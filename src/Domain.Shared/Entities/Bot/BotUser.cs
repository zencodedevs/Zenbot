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
        public DateTime Birthday { get; set; } = DateTime.MinValue;
        public DateTime NextNotifyTIme { get; set; } = DateTime.MinValue;

        public int GuildId { get; set; }
        public Guild Guild { get; set; }


        [NotMapped]
        public virtual ICollection<Vocation> Vocations { get; set; }



        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; }

        public BotUser Create(string username, string userMail, ulong userId, DateTime birthday, DateTime nextNotifyTime)
        {
            Birthday = birthday;
            NextNotifyTIme = nextNotifyTime;
            DiscordId = userId;
            UserMail = userMail;
            Username = username;
            return this;
        }
    }

    

    //public class Supervisor : ValueObject
    //{
    //    public ulong DiscordId { get; init; }
    //    public string Username { get; init; }

    //    public Supervisor(ulong discordId, string username)
    //    {
    //        DiscordId = discordId;
    //        Username = username;
    //    }

    //    protected override IEnumerable<object> GetAtomicValues()
    //    {
    //        yield return DiscordId;
    //        yield return Username;
    //    }
    //}
    
}
