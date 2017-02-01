using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Core.Args
{
    public class DateChangedEventArgs : EventArgs
    {
        public DateChangedEventArgs(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; }
    }
}
