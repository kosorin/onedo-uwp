using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneDo.View.Controls
{
    public sealed partial class TimePicker : ExtendedUserControl, IXBind<TimePickerViewModel>
    {
        public TimePickerViewModel VM => ViewModel as TimePickerViewModel;

        public TimeSpan Time
        {
            get { return (TimeSpan)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public static readonly DependencyProperty TimeProperty =
            DependencyProperty.Register(nameof(Time), typeof(TimeSpan), typeof(TimePicker), new PropertyMetadata(null, Time_Changed));

        private static void Time_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var picker = d as TimePicker;
            if (picker != null)
            {

            }
        }


        public double HoursAngle
        {
            get { return HoursForegroundSlice.EndAngle; }
            set { HoursForegroundSlice.EndAngle = value; }
        }

        public double MinutesAngle
        {
            get { return MinutesForegroundSlice.EndAngle; }
            set { MinutesForegroundSlice.EndAngle = value; }
        }


        private const double hoursRadius = 120;
        private const double hoursInnerRadius = 80;
        private const double minutesRadius = 160;
        private const double minutesInnerRadius = 120;

        private Brush hoursBackgroundAmBrush;
        private Brush hoursBackgroundPmBrush;
        private Brush hoursForegroundAmBrush;
        private Brush hoursForegroundPmBrush;
        private Brush minutesBackgroundBrush;
        private Brush minutesForegroundBrush;

        private Unit? manipulationUnit;

        private bool isAm;

        public TimePicker()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            hoursBackgroundAmBrush = new SolidColorBrush((Color)Resources["SystemListMediumColor"]);
            hoursForegroundAmBrush = new SolidColorBrush((Color)Resources["SystemAccentColor"]);
            hoursBackgroundPmBrush = new SolidColorBrush((Color)Resources["SystemAccentColor"]);
            hoursForegroundPmBrush = new SolidColorBrush((Color)Resources["SystemAccentColorDark1"]);
            minutesBackgroundBrush = new SolidColorBrush((Color)Resources["SystemListLowColor"]);
            minutesForegroundBrush = new SolidColorBrush((Color)Resources["SystemAccentColorLight1"]);

            HoursBackgroundSlice.Fill = hoursBackgroundAmBrush;
            HoursForegroundSlice.Fill = hoursForegroundAmBrush;
            MinutesBackgroundSlice.Fill = minutesBackgroundBrush;
            MinutesForegroundSlice.Fill = minutesForegroundBrush;
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
            SetHours(time?.Hours ?? 0);
            SetMinutes(time?.Minutes ?? 0);
        }

        private void SetHours(int hours)
        {
            isAm = hours < 12;
            HoursAngle = (hours % 12) * 30;
        }

        private void SetMinutes(int minutes)
        {
            MinutesAngle = minutes * 6;
        }

        private void UpdateTime()
        {
            VM.Time = new TimeSpan(HoursFromAngle(HoursAngle), MinutesFromAngle(MinutesAngle), 0);
        }


        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var distance = GetDistance(e.GetPosition(RootGrid), RootGrid.RenderSize);
            var unit = GetUnit(distance);
            if (unit != null)
            {
                var angle = GetAngle(e.GetPosition(RootGrid), RootGrid.RenderSize);
                CompleteManipulation((Unit)unit, angle);
            }
        }

        private void RootGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var distance = GetDistance(e.Position, RootGrid.RenderSize);
            manipulationUnit = GetUnit(distance);
        }

        private void RootGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (manipulationUnit == null)
            {
                return;
            }
            CompleteManipulation((Unit)manipulationUnit, manipulationUnit == Unit.Hours ? HoursAngle : MinutesAngle);
        }

        private void RootGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (manipulationUnit == null)
            {
                return;
            }

            var angle = GetAngle(e.Position, RootGrid.RenderSize);
            switch (manipulationUnit)
            {
            case Unit.Hours: HoursAngle = angle; break;
            case Unit.Minutes: MinutesAngle = angle; break;
            }
        }


        private void CompleteManipulation(Unit unit, double angle)
        {
            if (unit == Unit.Hours)
            {
                HoursAngle = HoursFromAngle(angle) * 30;
            }
            else if (unit == Unit.Minutes)
            {
                MinutesAngle = MinutesFromAngle(angle) * 6;
            }
        }


        private Unit? GetUnit(double distance)
        {
            if (distance < hoursInnerRadius) return null;
            if (distance < hoursRadius) return Unit.Hours;
            if (distance < minutesRadius) return Unit.Minutes;
            return null;
        }

        private double GetDistance(Point point, Size size)
        {
            var x = point.X - (size.Width / 2d);
            var y = size.Height - point.Y - (size.Height / 2d);
            return Math.Sqrt(x * x + y * y);
        }

        private double GetAngle(Point point, Size size)
        {
            var x = point.X - (size.Width / 2d);
            var y = size.Height - point.Y - (size.Height / 2d);
            var hypot = Math.Sqrt(x * x + y * y);
            var value = Math.Asin(y / hypot) * 180 / Math.PI;
            var quadrant = (x >= 0) ?
                (y >= 0) ? Quadrant.NorthEast : Quadrant.SouthEast :
                (y >= 0) ? Quadrant.NorthWest : Quadrant.SouthWest;

            switch (quadrant)
            {
            case Quadrant.NorthEast:
            case Quadrant.SouthEast: value = 90 - value; break;
            default: value = 270 + value; break;
            }

            return value;
        }

        private Quadrant QuadrantFromAngle(double angle)
        {
            if (angle < 90) return Quadrant.NorthEast;
            if (angle < 180) return Quadrant.SouthEast;
            if (angle < 270) return Quadrant.SouthWest;
            if (angle <= 360) return Quadrant.NorthWest;
            return Quadrant.NorthEast;
        }

        private int HoursFromAngle(double angle)
        {
            return (int)Math.Round(angle / 30d);
        }

        private int MinutesFromAngle(double angle)
        {
            return (int)Math.Round(angle / 6d);
        }


        private enum Quadrant
        {
            NorthEast = 1,
            NorthWest = 2,
            SouthEast = 3,
            SouthWest = 4,
        }

        private enum Unit
        {
            Hours,
            Minutes,
        }
    }
}
