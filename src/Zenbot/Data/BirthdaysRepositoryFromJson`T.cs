using Zenbot.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Zenbot.Data
{
    using UserId = String;

    /// <summary>
    /// Implements BirthdaysRepository loaded from Json file
    /// with optional caching support.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BirthdaysRepositoryFromJson<T> : IBirthdaysRepositoryCached where T : class, IBirthdaysCache, new()
    {
        private T _birthdaysCache;

        private readonly IConfiguration _config;
        private BirthdaysArray _deserializedBirthdays;
        private Stream _birthdaysJsonStream;

        public BirthdaysRepositoryFromJson(IConfiguration config, Stream birthdaysJsonStream)
        {
            _config = config;
            _birthdaysJsonStream = birthdaysJsonStream;
            _deserializedBirthdays = DeserialiseBirthdayConfig();

            _birthdaysCache = new();
        }

        private BirthdaysArray DeserialiseBirthdayConfig()
        {
            return JsonSerializer.Deserialize<BirthdaysArray>(new StreamReader(_birthdaysJsonStream).ReadToEnd());
        }

        /// <summary>
        /// Serializes current content of birthdays array to Json and writes it to file
        /// 
        /// </summary>
        private void WriteBirthdaysToJsonFile()
        {
            _birthdaysJsonStream.SetLength(0); // Makes it so that Writer overrides the file instead of appending

            JsonSerializerOptions serializerOptions = new() { WriteIndented = true };
            StreamWriter streamWriter = new(_birthdaysJsonStream);
            streamWriter.Write(JsonSerializer.Serialize<BirthdaysArray>(_deserializedBirthdays, serializerOptions));
            streamWriter.Flush();
        }

        public async Task AddUserBirthdayAsync(Birthday birthday)
        {
            try
            {
                await _birthdaysCache.AddUserBirthdayAsync(birthday);
            }
            catch (NotImplementedException)
            {
                // If this type of cache does not have Add implemented - skip painlessly
            }

            // Future implementation: have a cache of <operations>, bulk save to source every <time interval>
            // SaveToSourceAsync() used for bulk save
            // Perhaps, another Interface / class to implement the operations cache so it can be shared across different types of sources

            // Current implementation: edit file every time the method is called

            _deserializedBirthdays.Birthdays.Add(birthday);
            WriteBirthdaysToJsonFile();

            Console.WriteLine($"Added Birthday: {birthday.UserId} - {birthday.BirthdayDate.ToString()}");
        }

        public async Task AdjustUserBirthdayAsync(Birthday birthday)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserBirthdayAsync(Birthday birthday)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> LookupUsersByBirthdayAsync(DateTime birthdayDate, string serverId = null)
        {
            return await _birthdaysCache.LookupUsersByBirthdayAsync(birthdayDate, serverId);
        }

        public async Task LoadFromSourceAsync()
        {
            DateTime date = DateTime.Today;
            UserId id = new(String.Empty);

            var birthdaysRawData = _config.GetSection("Birthdays").Get<IConfigurationSection[]>();

            if (birthdaysRawData == null)
                return;

            foreach (var pairIdBirthday in birthdaysRawData)
            {
                if (pairIdBirthday["Date"].FromBirthdayFormat(out date))
                {
                    id = pairIdBirthday["Id"];

                    await _birthdaysCache.AddUserBirthdayAsync(new Birthday(id, date));

                    Console.WriteLine($"Birthday loaded: {pairIdBirthday["Id"]} - {date}");
                }
                else
                {
                    throw new FormatException("Could not parse one or more birthday dates from configuration. Did you use correct date format?");
                }
            }
        }


        public async Task SaveToSourceAsync()
        {
            throw new NotImplementedException();
        }
    }
}
