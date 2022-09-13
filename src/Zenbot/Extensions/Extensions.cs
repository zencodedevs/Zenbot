using System;
using System.Globalization;

namespace Zenbot.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToBirthdayFormat(this DateTime BirthdayDate)
        {
            return BirthdayDate.ToString(Constant._birthdayDateFormat);
        }
    }

    public static class StringExtensions
    {
        public static bool FromBirthdayFormat(this string BirthdayDate, out DateTime date)
        {
            return DateTime.TryParseExact(BirthdayDate, Constant._birthdayDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }
    }
}

