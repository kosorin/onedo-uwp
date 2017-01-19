using OneDo.Common.Extensions;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using System;
using Windows.UI.Xaml;

namespace OneDo.View
{
    public sealed partial class TimePicker : ExtendedUserControl, IXBind<TimePickerViewModel>
    {
        public TimePickerViewModel VM => ViewModel as TimePickerViewModel;

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


        public TimePicker()
        {
            InitializeComponent();
        }

        public void UpdateTime()
        {
            if (VM != null)
            {
                VM.Time = new TimeSpan((int)Hours, (int)Minutes, 0);
            }
        }

        protected override void OnViewModelChanging()
        {
            if (VM != null)
            {
                VM.TimeChanged -= OnTimeChanged;
            }
        }

        protected override void OnViewModelChanged()
        {
            if (VM != null)
            {
                OnTimeChanged(VM.Time);
                VM.TimeChanged += OnTimeChanged;
            }
        }


        private void OnTimeChanged(TimePickerViewModel picker, TimePickerEventArgs args)
        {
            OnTimeChanged(args.Time);
        }

        private void OnTimeChanged(TimeSpan? time)
        {
            var now = DateTime.Now.ToTime();
            Hours = time?.Hours ?? now.Hours;
            Minutes = time?.Minutes ?? now.Minutes;
        }
    }
}
