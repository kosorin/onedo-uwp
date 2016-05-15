using System;

namespace OneDo.Common.Event
{
    public class CancelEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
    }
}