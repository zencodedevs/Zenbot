using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCore.Extenstions
{
    public static class IntegerExtenstions
    {
        /// <summary>
        /// Managing the file size and calculate accurately 
        /// </summary>


        private static readonly string[] SizeSuffixes =
                  { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string ToSizeSuffix(this int value, int decimalPlaces = 1)
            => ((long)value).ToSizeSuffix(decimalPlaces);

        public static string ToSizeSuffix(this long value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + (-value).ToSizeSuffix(decimalPlaces); }

            int i = 0;
            decimal dValue = value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
    }
}
