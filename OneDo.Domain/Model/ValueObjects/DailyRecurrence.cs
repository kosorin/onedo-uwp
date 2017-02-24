using Newtonsoft.Json;
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
        [JsonConstructor]
        public DailyRecurrence(int every, DateTime? until) : base(every, until)
        {
        }

        public override IEnumerable<DateTime> GetOccurrences(DateTime from)
        {
            var occurrence = from;
            while (occurrence <= ActualUntil)
            {
                yield return occurrence;
                occurrence = occurrence.AddDays(Every);
            }
            yield break;
        }

        protected override bool EqualsCore(Recurrence other)
        {
            var dailyRecurrence = other as DailyRecurrence;
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
