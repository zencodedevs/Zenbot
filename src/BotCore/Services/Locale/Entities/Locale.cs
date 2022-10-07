using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Services.Locale
{
    public class Locale
    {

        private Dictionary<string, string> items;
        public Locale(Dictionary<string, string> items)
        {
            this.items = items;
        }
        public string this[string key]
        {
            get
            {
                string result;
                if (items.TryGetValue(key, out string value))
                {
                    result = value;
                }
                else
                {
                    result = key;
                };
                return result;
            }
        }
    }
}
