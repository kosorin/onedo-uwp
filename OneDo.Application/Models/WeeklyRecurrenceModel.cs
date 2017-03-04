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
    public class WeeklyRecurrenceModel : StandardRecurrenceModel
    {
        internal override Recurrence ToEntity()
        {
            return new WeeklyRecurrence(DaysOfWeek.None, Every, Until);
        }


        protected override bool EqualsCore(RecurrenceModel other)
        {
            var weeklyRecurrence = other as WeeklyRecurrenceModel;
            if (weeklyRecurrence != null)
            {
                return Every == weeklyRecurrence.Every
                    && Until == weeklyRecurrence.Until;
            }
            else
            {
                return false;
            }
        }

        protected override int GetHashCodeCore()
        {
            return this.GetHashCodeFromFields(Every, Until);
        }
    }
}