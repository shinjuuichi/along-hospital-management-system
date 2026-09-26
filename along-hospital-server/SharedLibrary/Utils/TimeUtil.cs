using System.Globalization;

namespace SharedLibrary.Utils
{
    public static class TimeUtil
    {
        public static TimeOnly FloorToHalfHour(this TimeOnly time)
        {
            var flooredMinutes = (time.Minute / 30) * 30;
            return new TimeOnly(time.Hour, flooredMinutes, 0);
        }

        public static DateTime FloorToHalfHour(this DateTime time)
        {
            var flooredMinutes = (time.Minute / 30) * 30;
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, flooredMinutes, 0);
        }

        public static int ToTotalMinutes(this TimeOnly time)
        {
            return time.Hour * 60 + time.Minute;
        }

        public static TimeZoneInfo GetTimeZoneFromId(string timezoneId = "Asia/Ho_Chi_Minh")
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        }

        public static DateTime ConvertTimeToTimeZone(this DateTime dateTime, string timezoneId = "Asia/Ho_Chi_Minh")
        {
            var timeZone = TimeUtil.GetTimeZoneFromId(timezoneId);
            return TimeZoneInfo.ConvertTime(dateTime, timeZone);
        }

        public static int GetWeekOfYear(DateTime date)
        {
            var calendar = CultureInfo.InvariantCulture.Calendar;
            var weekRule = CalendarWeekRule.FirstFourDayWeek;
            var firstDayOfWeek = DayOfWeek.Monday;
            return calendar.GetWeekOfYear(date, weekRule, firstDayOfWeek);
        }

        public static DateTime? TryConvertToDateTime(object? value)
        {
            if (value is DateTime dateTime) return dateTime.Date;
            if (value is DateOnly dateOnly) return dateOnly.ToDateTime(TimeOnly.MinValue);
            return null;
        }

        public static TimeOnly? TryConvertToTimeOnly(object? value)
        {
            if (value is TimeOnly timeOnly) return timeOnly;
            if (value is DateTime dateTime) return TimeOnly.FromDateTime(dateTime);
            if (value is TimeSpan timeSpan && timeSpan >= TimeSpan.Zero && timeSpan < TimeSpan.FromDays(1))
                return TimeOnly.FromTimeSpan(timeSpan);
            return null;
        }
    }
}