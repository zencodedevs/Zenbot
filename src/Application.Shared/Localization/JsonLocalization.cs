using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Zenbot.Application.Shared.Localization
{
    class JsonLocalization
    {
        public string Key { get; set; }
        public Dictionary<string, string> LocalizedValue = new Dictionary<string, string>();
    }

    public class JsonStringLocalizer : IStringLocalizer
    {
        List<JsonLocalization> localization = new List<JsonLocalization>();
        public JsonStringLocalizer()
        {
            //read all json file
            JsonSerializer serializer = new JsonSerializer();
            localization = JsonConvert.DeserializeObject<List<JsonLocalization>>(File.ReadAllText(@"Resources/localization.json"));
        }
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return localization.Where(l => l.LocalizedValue.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name)).Select(l => new LocalizedString(l.Key, l.LocalizedValue[CultureInfo.CurrentCulture.Name], true));
        }
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return new JsonStringLocalizer();
        }
        private string GetString(string name)
        {
            var query = localization.Where(l => l.LocalizedValue.Keys.Any(lv => lv == CultureInfo.CurrentCulture.Name));
            var value = query.FirstOrDefault(l => l.Key == name);
            return value?.LocalizedValue[CultureInfo.CurrentCulture.Name];
        }
    }
}
