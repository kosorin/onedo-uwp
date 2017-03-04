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
    public class MonthlyRecurrenceModel : StandardRecurrenceModel
    {
        internal override Recurrence ToEntity()
        {
            return new MonthlyRecurrence(Every, Until);
        }


        protected override bool EqualsCore(RecurrenceModel other)
        {
            var monthlyRecurrence = other as MonthlyRecurrenceModel;
            if (monthlyRecurrence != null)
            {
                return Every == monthlyRecurrence.Every
                    && Until == monthlyRecurrence.Until;
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