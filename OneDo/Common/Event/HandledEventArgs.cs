using System;

namespace OneDo.Common.Event
{
    public class HandledEventArgs : EventArgs
    {
        public bool Handled { get; set; }
    }
}
