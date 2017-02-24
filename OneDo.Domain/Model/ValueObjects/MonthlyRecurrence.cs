using Newtonsoft.Json;
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
        [JsonConstructor]
        public MonthlyRecurrence(int every, DateTime? until) : base(every, until)
        {
        }

        public override IEnumerable<DateTime> GetOccurrences(DateTime from)
        {
            var skip = 0;
            var occurrence = from;
            while (occurrence <= ActualUntil)
            {
                yield return occurrence;
                skip += Every;
                occurrence = from.AddMonths(skip);
            }
            yield break;
        }

        protected override bool EqualsCore(Recurrence other)
        {
            var monthlyRecurrence = other as WeeklyRecurrence;
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