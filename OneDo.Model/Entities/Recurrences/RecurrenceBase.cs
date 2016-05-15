using System;

namespace OneDo.Model.Entities.Recurrences
{
    public abstract class RecurrenceBase
    {
        public int Every { get; set; }

        public DateTime? Until { get; set; }
    }
}
