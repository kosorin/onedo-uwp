using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Controls
{
    public class CustomCalendarView : CalendarView
    {
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register(nameof(SelectedDate), typeof(DateTime?), typeof(CustomCalendarView), new PropertyMetadata(null, SelectedDate_Changed));

        private static void SelectedDate_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var calendarView = d as CalendarView;
            if (calendarView != null)
            {
                calendarView.SelectedDates.Clear();

                var date = e.NewValue as DateTime?;
                if (date != null)
                {
                    calendarView.SelectedDates.Add(new DateTimeOffset((DateTime)date));
                }
            }
        }

        private bool isChangingSelectedDate = false;

        public CustomCalendarView()
        {
            SelectedDatesChanged += OnSelectedDatesChanged;
        }

        private void OnSelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (!isChangingSelectedDate)
            {
                isChangingSelectedDate = true;

                if (SelectedDates.Any())
                {
                    SelectedDate = SelectedDates.First().Date;
                }
                else
                {
                    SelectedDate = null;
                }

                isChangingSelectedDate = false;
            }
        }

        private void OnSelectedDateChanged(DateTime? date)
        {
            if (!isChangingSelectedDate)
            {
                isChangingSelectedDate = true;

                SelectedDates.Clear();
                if (date != null)
                {
                    var dateTimeOffset = new DateTimeOffset((DateTime)date);
                    SelectedDates.Add(dateTimeOffset);
                }

                isChangingSelectedDate = false;
            }
        }
    }
}
