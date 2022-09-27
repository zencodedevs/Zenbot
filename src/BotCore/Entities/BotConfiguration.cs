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

    /// <summary>
    /// Here we have all configuration about zenbot
    /// We're retriving all these data from config.json which all environment variables
    /// </summary>
    public class BotConfiguration
    {
        //Bot Server common config
        public string BotToken { get; set; }
        public string Prefix { get; set; }
        public ulong MainGuildId { get; set; }

        // bot Scrin.io Config
        public ScrinIO ScrinIO { get; set; }


        // Bot New user config
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

    public class ScrinIO
    {
        public string Token { get; set; }
        public string HeaderName { get; set; }
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
