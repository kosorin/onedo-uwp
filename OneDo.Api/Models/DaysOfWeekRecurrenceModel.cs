using OneDo.Common;
using OneDo.Common.Extensions;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Models
{
    public class DaysOfWeekRecurrenceModel : StandardRecurrenceModel
    {
        public DaysOfWeek DaysOfWeek { get; set; }

        internal override Recurrence ToEntity()
        {
            return new WeeklyRecurrence(DaysOfWeek, Every, Until);
        }


        protected override bool EqualsCore(RecurrenceModel other)
        {
            var daysOfWeekRecurrence = other as DaysOfWeekRecurrenceModel;
            if (daysOfWeekRecurrence != null)
            {
                return Every == daysOfWeekRecurrence.Every
                    && Until == daysOfWeekRecurrence.Until
                    && DaysOfWeek == daysOfWeekRecurrence.DaysOfWeek;
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