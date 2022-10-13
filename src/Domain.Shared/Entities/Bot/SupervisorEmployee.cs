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
    public class SupervisorEmployee : Entity
    {
        public int SupervisorId { get; set; }

        [ForeignKey("SupervisorId")]
        public BotUser Supervisor { get; set; }

        public int? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public BotUser Employee { get; set; }

        public virtual ICollection<Vocation> Vocations { get; set; }
    }
}
