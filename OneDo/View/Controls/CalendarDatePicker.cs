using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WindowsCalendarDatePicker = Windows.UI.Xaml.Controls.CalendarDatePicker;

namespace OneDo.View.Controls
{
    [TemplatePart(Name = ClearButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = NextDayButtonPartName, Type = typeof(Button))]
    [TemplatePart(Name = PreviousDayButtonPartName, Type = typeof(Button))]
    public class CalendarDatePicker : WindowsCalendarDatePicker
    {
        private const string ClearButtonPartName = "PART_ClearButton";
        private const string NextDayButtonPartName = "PART_NextDayButton";
        private const string PreviousDayButtonPartName = "PART_PreviousDayButton";

        private Button clearButton;
        private Button nextDayButton;
        private Button previousDayButton;

        public CalendarDatePicker()
        {
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
    }
}
