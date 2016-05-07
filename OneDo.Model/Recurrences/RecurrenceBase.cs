using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Model.Recurrences
{
    public abstract class RecurrenceBase
    {
        public int Every { get; set; }

        public DateTime? Until { get; set; }
    }
}
