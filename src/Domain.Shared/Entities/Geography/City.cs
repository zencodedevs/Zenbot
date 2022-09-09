using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Zen.Domain.Entities.Entity;
using Zen.Domain.Events;
using ZenAchitecture.Domain.Shared.Events;

namespace ZenAchitecture.Domain.Shared.Entities.Geography
{
    public class City : Entity, IHasDomainEvent
    {

        public string Name { get; private set; }

        [NotMapped]
        public List<DomainEvent> DomainEvents { get; set; }

        public City Create(string name)
        {
            Name = name;
            DomainEvents ??= new List<DomainEvent>();
            DomainEvents.Add(new CityCreatedEvent(this, Guid.NewGuid()));
            return this;
        }



        public void UpdateInfo(string name) => Name = name;

        public City Copy()
        {
            var entity = new City
            {
                Name = this.Name

            };

            return entity;
        }

    }
}