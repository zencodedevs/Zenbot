using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zen.Domain.Entities.Entity;

namespace Zenbot.Domain.Shared.Entities.Bot
{
    public class BoardingFiles : Entity
    {
        public string FilePath { get; set; }

        public int BoardingMessageId { get; set; }
        [ForeignKey("BoardingMessageId")]
        public virtual BoardingMessage BoardingMessage { get; set; }
    }
}
