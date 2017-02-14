using OneDo.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.ValueObjects
{
    public class MonthlyRecurrence : Recurrence
    {
        public MonthlyRecurrence(int every, DateTime? until) : base(every, until)
        {
        }

        protected override bool EqualsCore(Recurrence other)
        {
            var monthly = other as WeeklyRecurrence;
            if (monthly != null)
            {
                return Every == monthly.Every
                    && Until == monthly.Until;
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