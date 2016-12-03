using System;

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
