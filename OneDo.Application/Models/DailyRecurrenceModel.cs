using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Domain.Model.ValueObjects;
using OneDo.Common.Extensions;

namespace OneDo.Application.Models
{
    public class DailyRecurrenceModel : StandardRecurrenceModel
    {
        internal override Recurrence ToEntity()
        {
            return new DailyRecurrence(Every, Until);
        }


        protected override bool EqualsCore(RecurrenceModel other)
        {
            var dailyRecurrence = other as DailyRecurrenceModel;
            if (dailyRecurrence != null)
            {
                return Every == dailyRecurrence.Every
                    && Until == dailyRecurrence.Until;
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