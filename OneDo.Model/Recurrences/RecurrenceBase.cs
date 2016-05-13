using System;

namespace OneDo.Model.Recurrences
{
    public abstract class RecurrenceBase
    {
        public int Every { get; set; }

        public DateTime? Until { get; set; }
    }
}
