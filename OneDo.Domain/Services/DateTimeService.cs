using System;
using Windows.Globalization.DateTimeFormatting;
using OneDo.Common.Extensions;

namespace OneDo.Domain.Services
{
    public class DateTimeService
    {
        public DateTimeFormatter LongDateFormatter { get; }

        public DateTimeFormatter ShortDateFormatter { get; }

        public DateTimeFormatter MonthDayFormatter { get; }

        public DateTimeService()
        {
            LongDateFormatter = new DateTimeFormatter("longdate");
            ShortDateFormatter = new DateTimeFormatter("shortdate");
            MonthDayFormatter = new DateTimeFormatter(YearFormat.None, MonthFormat.Numeric, DayFormat.Default, DayOfWeekFormat.Default);
        }


        public TimeSpan Time()
        {
            return DateTime.Now.ToTime(false);
        }

        public DateTime Yesterday() => DateTime.Today.Yesterday();

        public DateTime Today() => DateTime.Today;

        public DateTime Tomorrow() => DateTime.Today.Tomorrow();

        public DateTime ThisWeek()
        {
            var date = DateTime.Today;
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date;
            }
            return date.NextDayOfWeek(DayOfWeek.Sunday);
        }

        public DateTime NextWeek()
        {
            return ThisWeek().AddWeeks(1);
        }


        public bool IsTimeValid(TimeSpan time)
        {
            return time >= TimeSpan.Zero && time < TimeSpan.FromDays(1);
        }


        public DateTime CombineDateAndTime(DateTime date, TimeSpan time)
        {
            return date + time;
        }

        public DateTime CombineDateAndTime(DateTime date, TimeSpan? time)
        {
            return time != null
                ? date + (TimeSpan)time
                : date;
        }

        public DateTime? CombineDateAndTime(DateTime? date, TimeSpan? time)
        {
            return time != null
                ? date + (TimeSpan)time
                : date;
        }


        public string DateToLongString(DateTime? date)
        {
            if (date != null)
            {
                return DateToString((DateTime)date, LongDateFormatter);
            }
            return null;
        }

        public string DateToShortString(DateTime? date)
        {
            if (date != null)
            {
                if (((DateTime)date).IsThisYear())
                {
                    return DateToString((DateTime)date, MonthDayFormatter);
                }
                else
                {
                    return DateToString((DateTime)date, ShortDateFormatter);
                }
            }
            return null;
        }

        private string DateToString(DateTime date, DateTimeFormatter formatter)
        {
            date = date.Date;
            if (date == Today())
            {
                return "Today";
            }
            else if (date == Tomorrow())
            {
                return "Tomorrow";
            }
            else if (date == Yesterday())
            {
                return "Yesterday";
            }
            else if (date > Today().AddDays(1) && date < Today().AddWeeks(1))
            {
                return date.ToString("dddd");
            }
            return formatter?.Format(date);
        }


        public string TimeToString(TimeSpan? time)
        {
            if (time != null)
            {
                var timeValue = time.Value;
                if (IsTimeValid(timeValue))
                {
                    return CombineDateAndTime(Today(), timeValue).ToString("t");
                }
            }
            return null;
        }
    }
}
