using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Services.Locale
{
    public class LocaleService
    {
        private Dictionary<string, Locale> Locales;
        public LocaleService()
        {
            Locales = new Dictionary<string, Locale>();
        }
        public Locale Get(string key)
        {
            Locale locale;
            if (Locales.TryGetValue(key, out Locale value))
            {
                locale = value;
            }
            else
            {
                locale = Locales.FirstOrDefault(a => a.Key == "en-US").Value;
            }
            return locale;
        }
        public Task InitializeAsync()
        {
            //var currentPath = Directory.GetCurrentDirectory();
            //var localesPath = Path.Combine(currentPath, "Services/Locale/Locales");
            //var directoryPath = Directory.GetFiles(localesPath);

            //foreach (var f in directoryPath)
            //{
            //    var locale = Path.GetFileNameWithoutExtension(f);

            //    var data = File.ReadAllText(f);
            //    var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);

            //    Locales.Add(locale, new Locale(json));
            //}
            return Task.CompletedTask;
        }
    }
}
