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
        public int Every { get; }

        public DateTime? Until { get; }

        public Recurrence(int every, DateTime? until)
        {
            Every = every;
            Until = until;
        }
    }
}
