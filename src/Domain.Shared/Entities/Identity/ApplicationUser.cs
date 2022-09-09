using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Zen.Domain.Entities.Entity;
using Zen.Domain.Events;

namespace ZenAchitecture.Domain.Shared.Entities
{
    public class ApplicationUser : EntityUser, IHasDomainEvent
    {
        public ApplicationUser() { }

        public string Avatar { get; private set; }
    
        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; }

   
    }
}
