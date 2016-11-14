using OneDo.Common;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View.Controls
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
                if (date != null)
                {
                    var dateTimeOffset = new DateTimeOffset((DateTime)date);
                    CalendarView.SelectedDates.Add(dateTimeOffset);
                    CalendarView.SetDisplayDate(dateTimeOffset);
                }

                isPicking = false;
            }
        }
    }
}
