using Zenbot.Extensions;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zenbot.Data
{
    /// <summary>
    /// Class for storing Birthday data
    /// </summary>
    /// <remarks>
    /// Primary Key: userId
    /// <para>Data is stored separately per server to respect user privacy</para>
    /// <para>In case of serverId == null, data is applied to all servers without a server-specific setting</para>
    /// </remarks>
    public class Birthday
    {
        [JsonPropertyName("Id")]
        public string UserId { get; }

        [JsonPropertyName("Date")]
        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime BirthdayDate { get; }

        [JsonPropertyName("ServerId")]
        public string ServerId { get; }

        public Birthday(string userId, DateTime birthdayDate, string serverId = null)
        {
            UserId = userId;
            BirthdayDate = birthdayDate;
            ServerId = serverId;
        }
    }

    public class BirthdaysArray
    {
        [JsonPropertyName("Birthdays")]
        public List<Birthday> Birthdays { get; }

        public BirthdaysArray(List<Birthday> birthdays)
        {
            Birthdays = birthdays;
        }
    }

    /// <summary>
    /// Deserialising date from custom string format into regular DateTime
    /// </summary>
    public class DateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            reader.GetString()!.FromBirthdayFormat(out DateTime result);
            return result;
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.ToBirthdayFormat());
    }

}
