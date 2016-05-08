using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common.Event
{
    public class HandledEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }
}
