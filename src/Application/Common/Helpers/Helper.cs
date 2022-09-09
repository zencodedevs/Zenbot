using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace ZenAchitecture.Application.Common.Helpers
{
    public class Helper
    {
        public Helper()
        {

        }

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();

            var writer = new StreamWriter(stream);

            writer.Write(s);

            writer.Flush();

            stream.Position = 0;

            return stream;
        }


        public static string FileToBase64(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);

                var fileBytes = ms.ToArray();

                return Convert.ToBase64String(fileBytes);
            }
        }

    }
}
