using OneDo.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.ValueObjects
{
    public class WeeklyRecurrence : Recurrence
    {
        public DaysOfWeek DaysOfWeek { get; }

        public WeeklyRecurrence(DaysOfWeek daysOfWeek, int every, DateTime? until) : base(every, until)
        {
            DaysOfWeek = daysOfWeek;
        }

        protected override bool EqualsCore(Recurrence other)
        {
            var weekly = other as WeeklyRecurrence;
            if (weekly != null)
            {
                return Every == weekly.Every
                    && Until == weekly.Until
                    && DaysOfWeek == weekly.DaysOfWeek;
            }
            else
            {
                return false;
            }
        }

        protected override int GetHashCodeCore()
        {
            return this.GetHashCodeFromFields(Every, Until, DaysOfWeek);
        }
    }
}