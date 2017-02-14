using OneDo.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.ValueObjects
{
    public class DailyRecurrence : Recurrence
    {
        public DailyRecurrence(int every, DateTime? until) : base(every, until)
        {
        }

        protected override bool EqualsCore(Recurrence other)
        {
            var daily = other as DailyRecurrence;
            if (daily != null)
            {
                return Every == daily.Every
                    && Until == daily.Until;
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
