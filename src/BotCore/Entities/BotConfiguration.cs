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
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Here we have all configuration about zenbot
    /// We're retriving all these data from config.json which all environment variables
    /// </summary>
    public class BotConfiguration
    {
        private static IWebHostEnvironment _hostingEnvironment;

        public static bool IsInitialized { get; private set; }

        public static void Initialize(IWebHostEnvironment hostEnvironment)
        {
            if (IsInitialized)
                throw new InvalidOperationException("Object already initialized");

            _hostingEnvironment = hostEnvironment;
            IsInitialized = true;
        }

        //Bot Server common config
        public string BotToken { get; set; }
        public string Prefix { get; set; }
        public ulong loggerChannel { get; set; }

        // bot Scrin.io Config
        public ScrinIO ScrinIO { get; set; }
        
       
        // Read config data from apssetting.json in deferent environment
        public static BotConfiguration GetConfiguration(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            string data;
            switch (_hostingEnvironment.EnvironmentName)
            {
                case "Qa":
                    data = File.ReadAllText(@"appsettings.Qa.json");
                    break;
                case "Production":
                    data = File.ReadAllText(@"appsettings.Producttion.json");
                    break;
                case "Development":
                    data = File.ReadAllText(@"appsettings.Development.json");
                    break;

                default:
                    data = File.ReadAllText(@"appsettings.json");
                    break;
            }
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
