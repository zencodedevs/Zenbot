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
    public class Vocation : Entity
    {
        public DateTime RequestDate { get; set; }
        public DateTime ForDate { get; set; }
        public bool IsAccept { get; set; }

        public int UserRequestId { get; set; }

        [ForeignKey("UserRequestId")]
        public virtual BotUser UserRequest { get; set; }

        public int? SupervisorId { get; set; }

        [ForeignKey("SupervisorId")]
        public virtual BotUser Supervisor { get; set; }
    }
}
