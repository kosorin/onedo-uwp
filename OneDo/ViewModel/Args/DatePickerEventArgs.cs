using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Args
{
    public class DatePickerEventArgs : EventArgs
    {
        public DateTime? Date { get; }

        public DatePickerEventArgs(DateTime? date)
        {
            Date = date;
        }
    }
}
