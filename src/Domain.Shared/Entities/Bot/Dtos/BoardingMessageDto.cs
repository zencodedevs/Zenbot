using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot.Domain.Shared.Entities.Bot.Dtos
{
    public class BoardingMessageDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsActive { get; set; }
        public int GuildId { get; set; }
    }
}
