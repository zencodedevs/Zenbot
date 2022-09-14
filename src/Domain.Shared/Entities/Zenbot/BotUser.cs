using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared.Entities.Zenbot
{
    public class BotUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserMail { get; set; }
        public string Username { get; set; }
        public byte Month { get; set; }
        public byte Day { get; set; }

    }
}
