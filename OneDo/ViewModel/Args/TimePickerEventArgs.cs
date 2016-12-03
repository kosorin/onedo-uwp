using System;

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