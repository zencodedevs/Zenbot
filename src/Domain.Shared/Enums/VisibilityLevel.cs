using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace Zenbot.Domain.Shared.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum VisibilityLevel
    {
        ForMe,
        ForAdmins,
        ForEveryone
    };
}
