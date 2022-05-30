using System;

namespace TechDrum.Core.Utils
{
    public class TimeStampHelper
    {
        public static string GetTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmssffff");
        }
        public static DateTimeOffset ConvertTimeStamp(string timestamp)
        {
            int year = int.Parse(timestamp.Substring(0, 4));
            int month = int.Parse(timestamp.Substring(4, 2));
            int day = int.Parse(timestamp.Substring(6, 2));
            int hour = int.Parse(timestamp.Substring(8, 2));
            int minute = int.Parse(timestamp.Substring(10, 2));
            int second = int.Parse(timestamp.Substring(12, 2));
            DateTimeOffset dt = new DateTimeOffset(year, month, day, hour, minute, second, TimeSpan.Zero).UtcDateTime;
            return dt;
        }
    }
}
