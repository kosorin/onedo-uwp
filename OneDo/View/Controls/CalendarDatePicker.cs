using OneDo.Common.Extensions;
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
    public class CalendarDatePicker : WindowsCalendarDatePicker
    {
        private const string ClearButtonPartName = "ClearButton";
        private const string NextDayButtonPartName = "NextDayButton";
        private const string PreviousDayButtonPartName = "PreviousDayButton";
        private const string TodayButtonPartName = "TodayButton";
        private const string TomorrowButtonPartName = "TomorrowButton";

        private Button clearButton;
        private Button nextDayButton;
        private Button previousDayButton;
        private Button todayButton;
        private Button tomorrowButton;

        public CalendarDatePicker()
        {
            LoopingSelector
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (clearButton != null)
            {
                clearButton.Tapped -= ClearButton_Tapped;
            }
            clearButton = GetTemplateChild(ClearButtonPartName) as Button;
            if (clearButton != null)
            {
                clearButton.Tapped += ClearButton_Tapped;
            }

            if (nextDayButton != null)
            {
                nextDayButton.Tapped -= NextDayButton_Tapped;
            }
            nextDayButton = GetTemplateChild(NextDayButtonPartName) as Button;
            if (nextDayButton != null)
            {
                nextDayButton.Tapped += NextDayButton_Tapped;
            }

            if (previousDayButton != null)
            {
                previousDayButton.Tapped -= PreviousDayButton_Tapped;
            }
            previousDayButton = GetTemplateChild(PreviousDayButtonPartName) as Button;
            if (previousDayButton != null)
            {
                previousDayButton.Tapped += PreviousDayButton_Tapped;
            }

            if (todayButton != null)
            {
                todayButton.Tapped -= TodayButton_Tapped;
            }
            todayButton = GetTemplateChild(TodayButtonPartName) as Button;
            if (todayButton != null)
            {
                todayButton.Tapped += TodayButton_Tapped;
            }

            if (tomorrowButton != null)
            {
                tomorrowButton.Tapped -= TomorrowButton_Tapped;
            }
            tomorrowButton = GetTemplateChild(TomorrowButtonPartName) as Button;
            if (tomorrowButton != null)
            {
                tomorrowButton.Tapped += TomorrowButton_Tapped;
            }
        }

        private void ClearButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Date = null;
        }

        private void NextDayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Date = Date?.AddDays(1);
        }

        private void PreviousDayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Date = Date?.AddDays(-1);
        }

        private void TodayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsCalendarOpen = false;
            Date = DateTime.Today;
        }

        private void TomorrowButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            IsCalendarOpen = false;
            Date = DateTime.Today.Tomorrow();
        }
    }
}
