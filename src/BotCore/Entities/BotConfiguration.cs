using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;
    using Newtonsoft.Json;

    namespace BotCore
    {
        public class BotConfiguration
        {
            public string BotToken { get; set; }
            public ulong MainGuildId { get; set; }
            public ulong LoggerChannel { get; set; }
            public string Prefix { get; set; }

            public static BotConfiguration GetConfiguration()
            {
                var data = File.ReadAllText(@"config.json");
                var json = JsonConvert.DeserializeObject<BotConfiguration>(data);
                return json;
            }
        }
    }
}
