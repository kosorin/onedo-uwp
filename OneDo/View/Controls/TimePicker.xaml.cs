using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
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


        public float HoursAngle
        {
            get { return hourHandVisual.RotationAngleInDegrees; }
            set { hourHandVisual.RotationAngleInDegrees = value; }
        }

        public float MinutesAngle
        {
            get { return minuteHandVisual.RotationAngleInDegrees; }
            set { minuteHandVisual.RotationAngleInDegrees = value; }
        }


        private const float hoursRadius = 120;
        private const float hoursAmInnerRadius = 30;
        private const float hoursPmInnerRadius = 80;
        private const float minutesRadius = 170;
        private const float minutesInnerRadius = 120;

        private const float unitPartSize = 40;
        private const float hourAmHandLength = 40;
        private const float hourPmHandLength = 80;
        private const float minuteHandLength = 120;

        private Color accentColor;

        private Visual rootVisual;
        private ContainerVisual containerVisual;
        private SpriteVisual hourHandVisual;
        private SpriteVisual minuteHandVisual;
        private ImplicitAnimationCollection handImplicitAnimations;

        private Unit? manipulationUnit;

        public TimePicker()
        {
            InitializeComponent();
            InitializeColors();
            InitializeControls();

            RootCanvas.Loaded += (s, e) =>
            {
                InitializeComposition();
            };
        }

        private void InitializeColors()
        {
            accentColor = (Color)Resources["SystemAccentColor"];
        }

        private void InitializeControls()
        {
            var textBlockStyle = (Style)Resources["BodyTextBlockStyle"];
            for (int i = 0; i < 12; i++)
            {
                RootCanvas.Children.Add(new TextBlock
                {
                    Text = i.ToString(),
                    Style = textBlockStyle,
                    Tag = new NumberItem
                    {
                        Unit = Unit.Minutes,
                        Value = i,
                    },
                });
            }
        }

        private void InitializeComposition()
        {
            rootVisual = ElementCompositionPreview.GetElementVisual(RootCanvas);
            containerVisual = compositor.CreateContainerVisual();

            foreach (var textBlock in RootCanvas.Children.OfType<TextBlock>().Where(x => x.Tag is NumberItem).OrderBy(x => ((NumberItem)x.Tag).Value))
            {
                var item = (NumberItem)textBlock.Tag;

                var visual = ElementCompositionPreview.GetElementVisual(textBlock);
                var angle = (item.Value * 30d) - 90d;
                var angleInRadians = (angle / 180d) * Math.PI;

                var x = 0; // (minuteHandLength + unitPartSize) * (float)Math.Cos(angleInRadians);
                var y = 0; // (minuteHandLength + unitPartSize) * (float)Math.Sin(angleInRadians);

                visual.CenterPoint = new Vector3(-(float)textBlock.ActualWidth / 2, -(float)textBlock.ActualHeight / 2, 0);
                visual.Offset = new Vector3(((float)RootCanvas.Width / 2) + x, ((float)RootCanvas.Height / 2) + y, 0);
            }

            var handRotationAnimation = compositor.CreateScalarKeyFrameAnimation();
            handRotationAnimation.Duration = TimeSpan.FromMilliseconds(450);
            handRotationAnimation.Target = "RotationAngleInDegrees";
            handRotationAnimation.InsertExpressionKeyFrame(1, "this.FinalValue");

            handImplicitAnimations = compositor.CreateImplicitAnimationCollection();
            handImplicitAnimations["RotationAngleInDegrees"] = handRotationAnimation;

            hourHandVisual = compositor.CreateSpriteVisual();
            hourHandVisual.Size = new Vector2(4, hourPmHandLength);
            hourHandVisual.CenterPoint = new Vector3(hourHandVisual.Size.X / 2, hourHandVisual.Size.Y, 0);
            hourHandVisual.Offset = new Vector3(((float)RootCanvas.Width / 2) - (hourHandVisual.Size.X / 2), ((float)RootCanvas.Height / 2) - hourHandVisual.Size.Y, 0);
            hourHandVisual.Brush = compositor.CreateColorBrush(accentColor);
            hourHandVisual.ImplicitAnimations = handImplicitAnimations;

            minuteHandVisual = compositor.CreateSpriteVisual();
            minuteHandVisual.Size = new Vector2(2, minuteHandLength);
            minuteHandVisual.CenterPoint = new Vector3(minuteHandVisual.Size.X / 2, minuteHandVisual.Size.Y, 0);
            minuteHandVisual.Offset = new Vector3(((float)RootCanvas.Width / 2) - (minuteHandVisual.Size.X / 2), ((float)RootCanvas.Height / 2) - minuteHandVisual.Size.Y, 0);
            minuteHandVisual.Brush = compositor.CreateColorBrush(accentColor);
            minuteHandVisual.ImplicitAnimations = handImplicitAnimations;

            containerVisual.Children.InsertAtTop(minuteHandVisual);
            containerVisual.Children.InsertAtTop(hourHandVisual);
            ElementCompositionPreview.SetElementChildVisual(RootCanvas, containerVisual);
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
            var distance = GetDistance(e.GetPosition(RootCanvas), RootCanvas.RenderSize);
            var unit = GetUnit(distance);
            if (unit != null)
            {
                var angle = GetAngle(e.GetPosition(RootCanvas), RootCanvas.RenderSize);
                CompleteManipulation((Unit)unit, angle);
            }
        }

        private void RootGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            var distance = GetDistance(e.Position, RootCanvas.RenderSize);
            manipulationUnit = GetUnit(distance);
            if (manipulationUnit == null)
            {
                return;
            }
            switch (manipulationUnit)
            {
            case Unit.Hours: hourHandVisual.ImplicitAnimations = null; break;
            case Unit.Minutes: minuteHandVisual.ImplicitAnimations = null; break;
            }
        }

        private void RootGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (manipulationUnit == null)
            {
                return;
            }
            var angle = GetAngle(e.Position, RootCanvas.RenderSize);
            CompleteManipulation((Unit)manipulationUnit, angle);
        }

        private void RootGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (manipulationUnit == null)
            {
                return;
            }

            var angle = GetAngle(e.Position, RootCanvas.RenderSize);
            switch (manipulationUnit)
            {
            case Unit.Hours: HoursAngle = angle; break;
            case Unit.Minutes: MinutesAngle = angle; break;
            }
        }


        private void CompleteManipulation(Unit unit, float angle)
        {
            if (unit == Unit.Hours)
            {
                var targetAngle = HoursFromAngle(angle) * 30;
                if (targetAngle != HoursAngle)
                {
                    hourHandVisual.ImplicitAnimations = handImplicitAnimations;
                    HoursAngle = targetAngle;
                }
            }
            else if (unit == Unit.Minutes)
            {
                var targetAngle = MinutesFromAngle(angle) * 6;
                if (targetAngle != MinutesAngle)
                {
                    minuteHandVisual.ImplicitAnimations = handImplicitAnimations;
                    MinutesAngle = targetAngle;
                }
            }
        }


        private Unit? GetUnit(float distance)
        {
            if (distance < hoursAmInnerRadius) return null;
            if (distance < hoursRadius) return Unit.Hours;
            if (distance < minutesRadius) return Unit.Minutes;
            return null;
        }

        private float GetDistance(Point point, Size size)
        {
            var x = point.X - (size.Width / 2d);
            var y = size.Height - point.Y - (size.Height / 2d);
            return (float)Math.Sqrt(x * x + y * y);
        }

        private float GetAngle(Point point, Size size)
        {
            var x = point.X - (size.Width / 2d);
            var y = size.Height - point.Y - (size.Height / 2d);
            var hypot = Math.Sqrt(x * x + y * y);
            var value = (float)(Math.Asin(y / hypot) * 180 / Math.PI);
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

        private Quadrant QuadrantFromAngle(float angle)
        {
            if (angle < 90) return Quadrant.NorthEast;
            if (angle < 180) return Quadrant.SouthEast;
            if (angle < 270) return Quadrant.SouthWest;
            if (angle <= 360) return Quadrant.NorthWest;
            return Quadrant.NorthEast;
        }

        private int HoursFromAngle(float angle)
        {
            return ((int)Math.Round(angle / 30d)) % 12;
        }

        private int MinutesFromAngle(float angle)
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

        private class NumberItem
        {
            public Unit Unit { get; set; }

            public int Value { get; set; }
        }
    }
}
