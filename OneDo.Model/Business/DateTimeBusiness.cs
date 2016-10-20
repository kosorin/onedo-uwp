using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Common;

namespace OneDo.Model.Business
{
    public class DateTimeBusiness : DataBusinessBase
    {
        public DateTimeBusiness(DataService dataService) : base(dataService)
        {

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

        public string DateToString(DateTime? date, string nullString = "")
        {
            if (date != null)
            {
                var dateValue = ((DateTime)date).Date;

                if (dateValue == Today())
                {
                    return "Today";
                }
                else if (dateValue == Tomorrow())
                {
                    return "Tomorrow";
                }
                else if (dateValue == Yesterday())
                {
                    return "Yesterday";
                }
                else if (dateValue > Today().AddDays(1) && dateValue < Today().AddWeeks(1))
                {
                    return dateValue.ToString("dddd");
                }
                return dateValue.ToString("D");
            }
            return nullString;
        }

        public string TimeToString(TimeSpan? time, string nullString = "")
        {
            if (time != null)
            {
                var timeValue = time.Value;
                if (timeValue > TimeSpan.Zero)
                {
                    return (Today() + timeValue).ToString("t");
                }
            }
            return nullString;
        }
    }
}
