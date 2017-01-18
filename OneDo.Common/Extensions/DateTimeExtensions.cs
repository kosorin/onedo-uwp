using System;
using Windows.Globalization.DateTimeFormatting;

namespace OneDo.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static DateTimeFormatter longDateFormatter;

        private static DateTimeFormatter shortDateFormatter;

        private static DateTimeFormatter monthDayFormatter;

        static DateTimeExtensions()
        {
            longDateFormatter = new DateTimeFormatter("longdate");
            shortDateFormatter = new DateTimeFormatter("shortdate");
            monthDayFormatter = new DateTimeFormatter(YearFormat.None, MonthFormat.Numeric, DayFormat.Default, DayOfWeekFormat.Default);
        }


        public static DateTime Yesterday(this DateTime date)
        {
            return date.AddDays(-1);
        }

        public static DateTime Tomorrow(this DateTime date)
        {
            return date.AddDays(1);
        }


        public static DateTime AddWeeks(this DateTime date, int weeks)
        {
            return date.AddDays(7 * weeks);
        }


        public static DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }


        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek dayOfweek)
        {
            int diff = dayOfweek - date.DayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(diff).Date;
        }

        public static DateTime FirstDayOfWeek(this DateTime date, DayOfWeek startOfWeek)
        {
            int diff = date.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.AddDays(-diff).Date;
        }

        public static DateTime LastDayOfWeek(this DateTime date, DayOfWeek startOfWeek)
        {
            var firstDayOfWeek = FirstDayOfWeek(date, startOfWeek);
            return firstDayOfWeek.AddDays(6);
        }


        public static DateTime Floor(this DateTime dt, TimeSpan interval)
        {
            return dt.AddTicks(-(dt.Ticks % interval.Ticks));
        }

        public static DateTime Ceiling(this DateTime dt, TimeSpan interval)
        {
            return dt.AddTicks(interval.Ticks - (dt.Ticks % interval.Ticks));
        }

        public static DateTime Round(this DateTime dt, TimeSpan interval)
        {
            var halfIntervalTicks = ((interval.Ticks + 1) >> 1);
            return dt.AddTicks(halfIntervalTicks - ((dt.Ticks + halfIntervalTicks) % interval.Ticks));
        }


        public static string ToFileName(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd_HH-dd-ss");
        }

        public static string ToLongDateString(this DateTime date)
        {
            return DateToString(date, longDateFormatter);
        }

        public static string ToShortDateString(DateTime date)
        {
            if (date.Year == DateTime.Now.Year)
            {
                return DateToString(date, monthDayFormatter);
            }
            else
            {
                return DateToString(date, shortDateFormatter);
            }
        }

        private static string DateToString(DateTime date, DateTimeFormatter formatter)
        {
            date = date.Date;
            if (date == DateTime.Today)
            {
                return "Today";
            }
            else if (date == DateTime.Today.Tomorrow())
            {
                return "Tomorrow";
            }
            else if (date == DateTime.Today.Yesterday())
            {
                return "Yesterday";
            }
            else if (date > DateTime.Today.Tomorrow() && date < DateTime.Today.AddWeeks(1))
            {
                return date.ToString("dddd");
            }
            return formatter?.Format(date);
        }


        public static DateTime SetTime(this DateTime date, int hour, int minute)
        {
            return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
        }

        public static TimeSpan ToTime(this DateTime dateTime, bool includeSeconds)
        {
            return new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
        }
    }
}
