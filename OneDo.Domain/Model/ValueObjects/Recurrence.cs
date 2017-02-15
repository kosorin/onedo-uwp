using OneDo.Common.Extensions;
using OneDo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.ValueObjects
{
    public abstract class Recurrence : ValueObject<Recurrence>
    {
        private readonly DateTime maxUntil = DateTime.Today.AddYears(4);

        public int Every { get; }

        public DateTime? Until { get; }

        public DateTime ActualUntil { get; }

        public bool IsForever { get; }

        protected Recurrence(int every, DateTime? until)
        {
            if (every < 1)
            {
                throw new ArgumentException($"Recur every: {every} < 1", nameof(every));
            }

            Every = every;
            Until = until;

            ActualUntil = (Until ?? maxUntil).SetTime(23, 59);
            IsForever = Until == null;
        }

        public abstract IEnumerable<DateTime> GetOccurrences(DateTime from);
    }
}
