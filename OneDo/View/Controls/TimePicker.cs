using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace OneDo.View.Controls
{
    [TemplatePart(Name = nameof(TimeButton), Type = typeof(Button))]
    [ContentProperty(Name = nameof(Time))]
    public sealed class TimePicker : Control
    {
        public TimeSpan Time
        {
            get => (TimeSpan)GetValue(TimeProperty);
            set => SetValue(TimeProperty, value);
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(TimeSpan.Zero, OnTimeChanged));

        private static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TimePicker picker)
            {
                picker.OnTimeChanged();
            }
        }

        public event TypedEventHandler<TimePickerFlyout, TimePickedEventArgs> TimePicked;

        private Button TimeButton;

        public TimePicker()
        {
            DefaultStyleKey = typeof(TimePicker);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TimeButton = FindTemplateChild<Button>(nameof(TimeButton));

            SetNewFlyout();
        }

        private TChild FindTemplateChild<TChild>(string childName) where TChild : DependencyObject
        {
            return (GetTemplateChild(childName) as TChild) ?? throw new InvalidOperationException($"Cannot find template child '{childName}' ({typeof(TChild).Name}) ");
        }

        private void TimePickerFlyout_TimePicked(TimePickerFlyout sender, TimePickedEventArgs args)
        {
            TimePicked?.Invoke(sender, args);
            Time = args.NewTime;
        }

        private void OnTimeChanged()
        {
            TimeButton.Content = DateTime.Today.Add(Time).ToString("t");
            SetNewFlyout();
        }

        private void SetNewFlyout()
        {
            ClearOldFlyout();

            var newFlyout = new TimePickerFlyout();
            newFlyout.TimePicked += TimePickerFlyout_TimePicked;
            newFlyout.Placement = FlyoutPlacementMode.Bottom;
            newFlyout.MinuteIncrement = 5;
            newFlyout.Time = Time;
            TimeButton.Flyout = newFlyout;
        }

        private void ClearOldFlyout()
        {
            if (TimeButton.Flyout is TimePickerFlyout oldFlyout)
            {
                oldFlyout.TimePicked -= TimePickerFlyout_TimePicked;
            }
        }
    }
}
