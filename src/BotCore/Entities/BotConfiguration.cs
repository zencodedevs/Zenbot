namespace Zenbot.BotCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;
    using Newtonsoft.Json;

    public class BotConfiguration
    {
        public string BotToken { get; set; }
        public string Prefix { get; set; }
        public ulong MainGuildId { get; set; }


        public Channels Channels { get; set; }
        public Roles Roles { get; set; }
        public Text Text { get; set; }
        public Files StaticFiles { get; set; }


        public static BotConfiguration GetConfiguration()
        {
            var data = File.ReadAllText(@"config.json");
            var json = JsonConvert.DeserializeObject<BotConfiguration>(data);
            return json;
        }
    }
    public class Files
    {
        public string GreetingFile { get; set; }
    }
    public class Text
    {
        public string GreetingMessage { get; set; }
    }
    public class Channels
    {
        public ulong LoggerId { get; set; }
        public ulong AuthenticationId { get; set; }
    }
    public class Roles
    {
        public ulong VarifiedId { get; set; }
        public ulong UnVarifiedId { get; set; }
        public ulong HR { get; set; }
    }

}
