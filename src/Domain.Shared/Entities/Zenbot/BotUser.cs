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
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserMail { get; set; }
        public string Username { get; set; }
        public byte Month { get; set; }
        public byte Day { get; set; }

        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; }

        public BotUser Create(string username, string userMail, string userId, byte month, byte day)
        {
            Day = day;
            Month = month;
            UserId = userId;
            UserMail = userMail;
            Username = username;
            //DomainEvents ??= new List<DomainEvent>();
            //DomainEvents.Add(new BotUserCreatedEvent(this, Guid.NewGuid()));
            return this;
        }



        //public void UpdateInfo(string name) => Name = name;

        //public City Copy()
        //{
        //    var entity = new City
        //    {
        //        Name = this.Name

        //    };

        //    return entity;
        //}
    }
}
