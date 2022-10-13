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

    /// <summary>
    /// Here we have all configuration about zenbot
    /// We're retriving all these data from config.json which all environment variables
    /// </summary>
    public class BotConfiguration
    {
        //Bot Server common config
        public string BotToken { get; set; }
        public string Prefix { get; set; }

        // bot Scrin.io Config
        public ScrinIO ScrinIO { get; set; }
     
        public static BotConfiguration GetConfiguration()
        {
            var data = File.ReadAllText(@"appsettings.json");
            var json = JsonConvert.DeserializeObject<BotConfiguration>(data);
            return json;
        }
    }

    public class ScrinIO
    {
        public string Token { get; set; }
        public string HeaderName { get; set; }
    }

}
