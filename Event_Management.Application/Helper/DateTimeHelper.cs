﻿using Org.BouncyCastle.Utilities;

namespace Event_Management.Domain.Helper
{
    public class DateTimeHelper
    {
        public static DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static long epochTime = dateTime.Ticks;
        public static DateTime GetDateTimeNow()
        {
            // Lấy múi giờ hiện tại của server
            DateTime serverTime = DateTime.UtcNow;

            // Tìm thông tin múi giờ của Việt Nam
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển múi giờ của server sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime, vietnamTimeZone);

            return vietnamTime;
        }

        public static string GetDateNow()
        {
            // Lấy múi giờ hiện tại của server
            DateTime serverTime = DateTime.UtcNow;

            // Tìm thông tin múi giờ của Việt Nam
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Chuyển múi giờ của server sang múi giờ của Việt Nam
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime, vietnamTimeZone);

            return vietnamTime.ToString("dd/MM/yyyy");
        }
        public static bool ValidateStartTimeAndEndTime(DateTime startTime, DateTime endTime)
        {
            long startTimeTick = startTime.AddMinutes(30).Ticks;
            long endTimeTick = endTime.Ticks;
            long now = DateTime.Now.Ticks;
            return startTimeTick > now && endTimeTick > startTimeTick;
        }
        public static DateTime ToDateTime(long tick)
        {
            DateTimeOffset value = DateTimeOffset.FromUnixTimeMilliseconds(tick);
            return value.DateTime;
        }
        public static string GetCronExpression(DateTime dateTime)
        {
            // The Cron expression format is: "seconds minutes hours dayOfMonth month dayOfWeek year"
            // Example: "0 0 12 * * ? *" (every day at 12:00 PM)
            return $"{dateTime.Second} {dateTime.Minute} {dateTime.Hour} {dateTime.Day} {dateTime.Month} ? {dateTime.Year}";
        }
    }
}
