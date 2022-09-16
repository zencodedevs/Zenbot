using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenbot
{
    public class BUser
    {
        public BUser()
        {
            this.Brithday = DateTime.MinValue;
        }
        public ulong Id { get; set; }
        public DateTime Brithday { get; set; }
        public bool NoticeBrithday { get; set; } = false;
    }
}
