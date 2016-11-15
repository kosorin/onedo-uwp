using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Common;
using System.Globalization;
using System.Text.RegularExpressions;
using Windows.Globalization.DateTimeFormatting;

namespace OneDo.Model.Business
{
    public class DateTimeBusiness : DataBusinessBase
    {
        public DateTimeFormatter LongDateFormatter { get; }

        public DateTimeFormatter ShortDateFormatter { get; }

        public DateTimeFormatter MonthDayFormatter { get; }

        public DateTimeBusiness(DataService dataService) : base(dataService)
        {
            LongDateFormatter = new DateTimeFormatter("longdate");
            ShortDateFormatter = new DateTimeFormatter("shortdate");
            MonthDayFormatter = new DateTimeFormatter(YearFormat.None, MonthFormat.Numeric, DayFormat.Default, DayOfWeekFormat.Default);
        }


        public DateTime Yesterday() => DateTime.Today.AddDays(-1);

        public DateTime Today() => DateTime.Today;

        public DateTime Tomorrow() => DateTime.Today.AddDays(1);

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
                if (timeValue > TimeSpan.Zero)
                {
                    return (Today() + timeValue).ToString("t");
                }
            }
            return null;
        }
    }
}
