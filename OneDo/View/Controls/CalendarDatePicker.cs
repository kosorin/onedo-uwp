using OneDo.Common.Extensions;
using OneDo.ViewModel.Core.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using WindowsCalendarDatePicker = Windows.UI.Xaml.Controls.CalendarDatePicker;

namespace OneDo.View.Controls
{
    [TemplatePart(Name = ClearButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = NextDayButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = PreviousDayButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = TodayButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = TomorrowButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = EndOfWeekButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = EndOfNextWeekButtonPartName, Type = typeof(Button))]
    public class CalendarDatePicker : WindowsCalendarDatePicker
    {
        private const string ClearButtonPartName = "ClearButton";
        private const string NextDayButtonPartName = "NextDayButton";
        private const string PreviousDayButtonPartName = "PreviousDayButton";
        private const string TodayButtonPartName = "TodayButton";
        private const string TomorrowButtonPartName = "TomorrowButton";
        private const string EndOfWeekButtonPartName = "EndOfWeekButton";
        private const string EndOfNextWeekButtonPartName = "EndOfNextWeekButton";


        public object UnselectedHeader
        {
            get { return (object)GetValue(UnselectedHeaderProperty); }
            set { SetValue(UnselectedHeaderProperty, value); }
        }
        public static readonly DependencyProperty UnselectedHeaderProperty =
            DependencyProperty.Register(nameof(UnselectedHeader), typeof(object), typeof(CalendarDatePicker), new PropertyMetadata(null));


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var clearButton = GetTemplateChild(ClearButtonPartName) as Button;
            var nextDayButton = GetTemplateChild(NextDayButtonPartName) as Button;
            var previousDayButton = GetTemplateChild(PreviousDayButtonPartName) as Button;
            var todayButton = GetTemplateChild(TodayButtonPartName) as Button;
            var tomorrowButton = GetTemplateChild(TomorrowButtonPartName) as Button;
            var endOfWeekButton = GetTemplateChild(EndOfWeekButtonPartName) as Button;
            var endOfNextWeekButton = GetTemplateChild(EndOfNextWeekButtonPartName) as Button;

            if (clearButton != null)
            {
                clearButton.Command = new RelayCommand(Clear);
            }
            if (nextDayButton != null)
            {
                nextDayButton.Command = new RelayCommand(NextDay);
            }
            if (previousDayButton != null)
            {
                previousDayButton.Command = new RelayCommand(PreviousDay);
            }
            if (todayButton != null)
            {
                todayButton.Command = new RelayCommand(Today);
            }
            if (tomorrowButton != null)
            {
                tomorrowButton.Command = new RelayCommand(Tomorrow);
            }
            if (endOfWeekButton != null)
            {
                endOfWeekButton.Command = new RelayCommand(EndOfWeek);
            }
            if (endOfNextWeekButton != null)
            {
                endOfNextWeekButton.Command = new RelayCommand(EndOfNextWeek);
            }
        }

        private void Clear()
        {
            Date = null;
        }

        private void NextDay()
        {
            Date = Date?.AddDays(1);
        }

        private void PreviousDay()
        {
            Date = Date?.AddDays(-1);
        }

        private void Today()
        {
            IsCalendarOpen = false;
            Date = DateTime.Today;
        }

        private void Tomorrow()
        {
            IsCalendarOpen = false;
            Date = DateTime.Today.Tomorrow();
        }

        private void EndOfWeek()
        {
            IsCalendarOpen = false;
            Date = DateTime.Today.LastDayOfWeek(DayOfWeek.Monday);
        }

        private void EndOfNextWeek()
        {
            IsCalendarOpen = false;
            Date = DateTime.Today.LastDayOfWeek(DayOfWeek.Monday).AddWeeks(1);
        }
    }
}
