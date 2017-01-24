using OneDo.Common.Args;
using OneDo.Common.Extensions;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Controls
{
    public sealed partial class TimePicker : ModalView
    {
        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(TimeSpan.Zero, Time_Changed));
        private static void Time_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = (TimePicker)d;
            var time = (TimeSpan)e.NewValue;
            picker.SetTime(time);
            picker.OnTimeChanged(time);
        }

        public double Hours
        {
            get { return (double)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register(nameof(Hours), typeof(double), typeof(TimePicker), new PropertyMetadata(0d));

        public double Minutes
        {
            get { return (double)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register(nameof(Minutes), typeof(double), typeof(TimePicker), new PropertyMetadata(0d));

        public event TypedEventHandler<TimePicker, TimeChangedEventArgs> TimeChanged;


        public TimePicker(TimeSpan time)
        {
            InitializeComponent();

            Time = time;
        }

        public void UpdateTime()
        {
            Time = new TimeSpan((int)Hours, (int)Minutes, 0);
        }

        public void SetInOneMinute()
        {
            Time = DateTime.Now.ToTime() + TimeSpan.FromMinutes(1);
        }

        public void SetInFiveMinutes()
        {
            Time = DateTime.Now.ToTime() + TimeSpan.FromMinutes(5);
        }

        private void SetTime(TimeSpan time)
        {
            Hours = time.Hours;
            Minutes = time.Minutes;
        }


        private void OnTimeChanged(TimeSpan time)
        {
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(time));
        }
    }
}
