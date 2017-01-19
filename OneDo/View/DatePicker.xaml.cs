using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace OneDo.View
{
    public sealed partial class DatePicker : ExtendedUserControl, IXBind<DatePickerViewModel>
    {
        public DatePickerViewModel VM => ViewModel as DatePickerViewModel;

        private bool isPicking = false;

        public DatePicker()
        {
            InitializeComponent();
        }

        protected override void OnViewModelChanging()
        {
            if (VM != null)
            {
                VM.DateChanged -= OnDateChanged;
            }
        }

        protected override void OnViewModelChanged()
        {
            if (VM != null)
            {
                SetCalendarViewDate(VM.Date);
                VM.DateChanged += OnDateChanged;
            }
        }

        private void OnDateChanged(DatePickerViewModel picker, DatePickerEventArgs args)
        {
            SetCalendarViewDate(args.Date);
        }

        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (!isPicking && VM != null)
            {
                if (args.AddedDates.Any())
                {
                    isPicking = true;
                    VM.Date = args.AddedDates.First().Date;
                    isPicking = false;
                }
                else
                {
                    VM.Refresh();
                    SetCalendarViewDate(VM.Date);
                }
            }
        }

        private void SetCalendarViewDate(DateTime? date)
        {
            if (!isPicking)
            {
                isPicking = true;

                CalendarView.SelectedDates.Clear();
                var dateTimeOffset = new DateTimeOffset(date ?? DateTime.Today);
                CalendarView.SelectedDates.Clear();
                CalendarView.SelectedDates.Add(dateTimeOffset);
                CalendarView.SetDisplayDate(dateTimeOffset);

                isPicking = false;
            }
        }
    }
}
