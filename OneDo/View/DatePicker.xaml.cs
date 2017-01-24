using OneDo.Common.Args;
using OneDo.Common.Extensions;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View
{
    public sealed partial class DatePicker : ModalView
    {
        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(DateTime), typeof(DatePicker), new PropertyMetadata(DateTime.Today, Date_Changed));
        private static void Date_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (DatePicker)d;
            var date = (DateTime)e.NewValue;
            picker.SelectCalendarViewDate(date);
            picker.OnDateChanged(date);
        }

        public event TypedEventHandler<DatePicker, DateChangedEventArgs> DateChanged;


        private bool isPicking;

        public DatePicker(DateTime date) : base(null)
        {
            InitializeComponent();

            Date = date;
        }

        public void SetToday()
        {
            Date = DateTime.Today;
        }

        public void SetTomorrow()
        {
            Date = DateTime.Today.Tomorrow();
        }

        public void SetEndOfWeek()
        {
            Date = DateTime.Today.LastDayOfWeek(DayOfWeek.Monday);
        }

        public void SetEndOfNextWeek()
        {
            Date = DateTime.Today.LastDayOfWeek(DayOfWeek.Monday).AddWeeks(1);
        }

        private void SelectCalendarViewDate(DateTime date)
        {
            if (!isPicking)
            {
                isPicking = true;

                var dateOffset = new DateTimeOffset(date);
                CalendarView.SelectedDates.Clear();
                CalendarView.SelectedDates.Add(dateOffset);
                CalendarView.SetDisplayDate(dateOffset);

                isPicking = false;
            }
        }


        private void OnDateChanged(DateTime date)
        {
            DateChanged?.Invoke(this, new DateChangedEventArgs(date));
        }

        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (!isPicking)
            {
                if (args.AddedDates.Any())
                {
                    isPicking = true;
                    Date = args.AddedDates.First().Date;
                    isPicking = false;
                }
                else
                {
                    SelectCalendarViewDate(Date);
                    OnDateChanged(Date);
                }
            }
        }
    }
}
