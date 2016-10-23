using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Args
{
    public class TimePickerEventArgs : EventArgs
    {
        public TimeSpan? Time { get; }

        public TimePickerEventArgs(TimeSpan? time)
        {
            Time = time;
        }
    }
}