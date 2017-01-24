using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Args
{
    public class TimeChangedEventArgs : EventArgs
    {
        public TimeChangedEventArgs(TimeSpan time)
        {
            Time = time;
        }

        public TimeSpan Time { get; }
    }
}