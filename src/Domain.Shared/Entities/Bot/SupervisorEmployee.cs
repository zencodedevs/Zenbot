using Domain.Shared.Entities.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Domain.Shared.Entities.Bot
{
    public class SupervisorEmployee
    {
        public int SupervisorId { get; set; }
        public BotUser Supervisor { get; set; }

        public int EmployeeId { get; set; }
        public BotUser Employee { get; set; }

        public virtual ICollection<Vocation> Vocations { get; set; }
    }
}
