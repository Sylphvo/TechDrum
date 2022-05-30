using Invedia.Core.DateTimeUtils;
using Microsoft.AspNetCore.Http;
using TechDrum.Core.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace TechDrum.Core.Utils
{
    public static class CoreHelper
    {
        public static string GenFundistHmac(string secretKey, IDictionary<string, dynamic> message)
        {
            SHA256 hash = new SHA256Managed();
            var hashSecret = hash.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
            var sortedMessage = new SortedDictionary<string, dynamic>(message);
            var value = "";
            foreach (var (key, o) in sortedMessage)
            {
                if (key.Equals("hmac")) continue;
                value += o;
            }

            HMAC hmac = new HMACSHA256(hashSecret);
            var hmacByte = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            var hmacValue = "";
            foreach (var b in hmacByte)
            {
                hmacValue = string.Concat(hmacValue, b.ToString("x2"));
            }

            return hmacValue.ToLower();
        }

        public static HttpContext CurrentHttpContext =>
            Invedia.Web.Middlewares.HttpContextMiddleware.HttpContext.Current;

        public static TimeZoneInfo SystemTimeZoneInfo => DateTimeHelper.GetTimeZoneInfo(Formattings.TimeZone);

        public static DateTimeOffset SystemTimeNow => DateTimeOffset.UtcNow;

        public static DateTimeOffset UtcTimeNow => DateTimeOffset.UtcNow;

        public static DateTime UtcToSystemTime(this DateTimeOffset dateTimeOffsetUtc)
        {
            return dateTimeOffsetUtc.UtcDateTime.UtcToSystemTime();
        }

        public static DateTime UtcToSystemTime(this DateTime dateTimeUtc)
        {
            var dateTimeWithTimeZone = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, SystemTimeZoneInfo);

            return dateTimeWithTimeZone;
        }

        public static string GetIpClient()
        {
            return CurrentHttpContext.Connection.RemoteIpAddress.ToString();
        }

        public static int GetWeekOfMonth(this DateTime time)
        {
            DateTime first = new DateTime(time.Year, time.Month, 1);
            return time.GetWeekOfYear() - first.GetWeekOfYear() + 1;
        }

        public static int GetWeekOfYear(this DateTime time)
        {
            GregorianCalendar _gc = new GregorianCalendar();

            return _gc.GetWeekOfYear(time, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }
}