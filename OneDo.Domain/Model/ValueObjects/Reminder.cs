using Newtonsoft.Json;
using OneDo.Common.Extensions;
using OneDo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.ValueObjects
{
    public class Reminder : ValueObject<Reminder>
    {
        public DateTime DateTime { get; }

        public Recurrence Recurrence { get; }

        [JsonConstructor]
        public Reminder(DateTime dateTime, Recurrence recurrence)
        {
            if (dateTime == null)
            {
                throw new ArgumentNullException(nameof(dateTime));
            }

            DateTime = dateTime;
            Recurrence = recurrence;
        }

        public IEnumerable<DateTime> GetOccurrences()
        {
            if (Recurrence == null)
            {
                return DateTime.Yield();
            }
            return Recurrence.GetOccurrences(DateTime);
        }


        protected override bool EqualsCore(Reminder other)
        {
            return DateTime == other.DateTime && Recurrence == other.Recurrence;
        }

        protected override int GetHashCodeCore()
        {
            return this.GetHashCodeFromFields(DateTime, Recurrence);
        }
    }
}