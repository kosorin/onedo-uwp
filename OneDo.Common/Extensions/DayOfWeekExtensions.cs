using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Extensions
{
    public static class DayOfWeekExtensions
    {
        public static bool Has(this DaysOfWeek daysOfWeek, DayOfWeek dayOfWeek)
        {
            return IsIn(dayOfWeek, daysOfWeek);
        }

        public static bool IsIn(this DayOfWeek dayOfWeek, DaysOfWeek daysOfWeek)
        {
            switch (dayOfWeek)
            {
            case DayOfWeek.Monday: return (daysOfWeek & DaysOfWeek.Monday) != 0;
            case DayOfWeek.Tuesday: return (daysOfWeek & DaysOfWeek.Tuesday) != 0;
            case DayOfWeek.Wednesday: return (daysOfWeek & DaysOfWeek.Wednesday) != 0;
            case DayOfWeek.Thursday: return (daysOfWeek & DaysOfWeek.Thursday) != 0;
            case DayOfWeek.Friday: return (daysOfWeek & DaysOfWeek.Friday) != 0;
            case DayOfWeek.Saturday: return (daysOfWeek & DaysOfWeek.Saturday) != 0;
            case DayOfWeek.Sunday: return (daysOfWeek & DaysOfWeek.Sunday) != 0;
            default: return false;
            }
        }
    }
}
