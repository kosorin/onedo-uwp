using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        private Button TimeButton;

        public TimePicker()
        {
            DefaultStyleKey = typeof(TimePicker);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TimeButton = FindTemplateChild<Button>(nameof(TimeButton));
            TimeButton.Flyout = CreateFlyout();
        }

        private TChild FindTemplateChild<TChild>(string childName) where TChild : DependencyObject
        {
            return (GetTemplateChild(childName) as TChild) ?? throw new InvalidOperationException($"Cannot find template child '{childName}' ({typeof(TChild).Name}) ");
        }

        private TimePickerFlyout CreateFlyout()
        {
            var flyout = new TimePickerFlyout();
            flyout.TimePicked += TimePickerFlyout_TimePicked;
            flyout.Placement = FlyoutPlacementMode.Bottom;
            return flyout;
        }

        private void TimePickerFlyout_TimePicked(TimePickerFlyout sender, TimePickedEventArgs args)
        {
            Time = args.NewTime;
        }

        private void OnTimeChanged()
        {
            TimeButton.Content = DateTime.Today.Add(Time).ToString("t");
        }
    }
}
