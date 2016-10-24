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

        public float HoursAngle
        {
            get { return hourHandVisual.RotationAngleInDegrees; }
            set
            {
                var angle = FixAngle(HoursAngle, value, Unit.Hours);
                hourHandVisual.RotationAngleInDegrees = angle;
                hourIndicatorVisual.RotationAngleInDegrees = angle;
            }
        }

        public float MinutesAngle
        {
            get { return minuteHandVisual.RotationAngleInDegrees; }
            set
            {
                var angle = FixAngle(MinutesAngle, value, Unit.Minutes);
                minuteHandVisual.RotationAngleInDegrees = angle;
                minuteIndicatorVisual.RotationAngleInDegrees = angle;
            }
        }


        private const float hourRadius = 120;
        private const float hourAmInnerRadius = 50;
        private const float hourPmInnerRadius = 80;
        private const float minuteRadius = 160;

        private const float hourAmHandOffsetDistance = 50;
        private const float hourPmHandOffsetDistance = 80;
        private const float minuteHandOffsetDistance = 120;

        private const float unitPartSize = 40;

        private Color accentColor;

        private ContainerVisual containerVisual;
        private SpriteVisual hourHandVisual;
        private SpriteVisual minuteHandVisual;
        private Visual hourIndicatorVisual;
        private Visual minuteIndicatorVisual;
        private ImplicitAnimationCollection handImplicitAnimations;

        private bool isManipulating = false;
        private Unit? manipulationUnit = null;

        public TimePicker()
        {
            InitializeComponent();
            InitializeColors();
            InitializeCompositionAnimations();
            InitializeNumberTextBlocks();
            InitializeComposition();
        }

        private void InitializeColors()
        {
            accentColor = (Color)Resources["SystemAccentColor"];
        }

        private void InitializeNumberTextBlocks()
        {
            for (int i = 0, a = 0; i < 12; i++, a += 30)
            {
                RootCanvas.Children.Add(CreateNumberTextBlock(i, a, hourAmHandOffsetDistance));
            }
            for (int i = 12, a = 0; i < 24; i++, a += 30)
            {
                RootCanvas.Children.Add(CreateNumberTextBlock(i, a, hourPmHandOffsetDistance));
            }
            for (int i = 0, a = 0; i < 60; i += 5, a += 30)
            {
                RootCanvas.Children.Add(CreateNumberTextBlock(i, a, minuteHandOffsetDistance));
            }
        }

        private TextBlock CreateNumberTextBlock(int value, float angle, float distanceFromCenter)
        {
            var textBlock = new TextBlock
            {
                Text = value.ToString(),
                Style = (Style)Resources["BodyTextBlockStyle"],
            };
            textBlock.Loaded += (s, e) =>
            {
                var angleInRadians = ((angle - 90) / 180d) * Math.PI;

                var x = (distanceFromCenter + (unitPartSize / 2)) * (float)Math.Cos(angleInRadians);
                var y = (distanceFromCenter + (unitPartSize / 2)) * (float)Math.Sin(angleInRadians);
                x += ((float)RootCanvas.Width / 2) - ((float)textBlock.ActualWidth / 2);
                y += ((float)RootCanvas.Height / 2) - ((float)textBlock.ActualHeight / 2);

                var visual = ElementCompositionPreview.GetElementVisual(textBlock);
                visual.Offset = new Vector3(x, y, 0);
            };
            return textBlock;
        }

        private void InitializeComposition()
        {
            hourHandVisual = compositor.CreateSpriteVisual();
            hourHandVisual.Brush = compositor.CreateColorBrush(accentColor);
            hourHandVisual.ImplicitAnimations = handImplicitAnimations;

            hourIndicatorVisual = ElementCompositionPreview.GetElementVisual(HourIndicator);
            hourIndicatorVisual.ImplicitAnimations = handImplicitAnimations;

            UpdateHourVisual(false);


            minuteHandVisual = compositor.CreateSpriteVisual();
            minuteHandVisual.Size = new Vector2(2, minuteHandOffsetDistance);
            minuteHandVisual.CenterPoint = new Vector3(minuteHandVisual.Size.X / 2, minuteHandVisual.Size.Y, 0);
            minuteHandVisual.Offset = new Vector3(((float)RootCanvas.Width / 2) - (minuteHandVisual.Size.X / 2), ((float)RootCanvas.Height / 2) - minuteHandVisual.Size.Y, 0);
            minuteHandVisual.Brush = compositor.CreateColorBrush(accentColor);
            minuteHandVisual.ImplicitAnimations = handImplicitAnimations;

            minuteIndicatorVisual = ElementCompositionPreview.GetElementVisual(MinuteIndicator);
            minuteIndicatorVisual.CenterPoint = new Vector3((float)MinuteIndicator.Width / 2, (float)MinuteIndicator.Height + minuteHandOffsetDistance, 0);
            minuteIndicatorVisual.Offset = new Vector3(((float)RootCanvas.Width / 2) - ((float)MinuteIndicator.Width / 2), ((float)RootCanvas.Height / 2) - (float)MinuteIndicator.Height - minuteHandOffsetDistance, 0);
            minuteIndicatorVisual.ImplicitAnimations = handImplicitAnimations;


            containerVisual = compositor.CreateContainerVisual();
            containerVisual.Children.InsertAtTop(minuteHandVisual);
            containerVisual.Children.InsertAtTop(hourHandVisual);
            ElementCompositionPreview.SetElementChildVisual(RootCanvas, containerVisual);
        }

        private void InitializeCompositionAnimations()
        {
            var handRotationAnimation = compositor.CreateScalarKeyFrameAnimation();
            handRotationAnimation.Duration = TimeSpan.FromMilliseconds(450);
            handRotationAnimation.Target = "RotationAngleInDegrees";
            handRotationAnimation.InsertExpressionKeyFrame(1, "this.FinalValue");

            var handSizeAnimation = compositor.CreateVector2KeyFrameAnimation();
            handSizeAnimation.Duration = TimeSpan.FromMilliseconds(450);
            handSizeAnimation.Target = "Size";
            handSizeAnimation.InsertExpressionKeyFrame(1, "this.FinalValue");

            var handCenterPointAnimation = compositor.CreateVector3KeyFrameAnimation();
            handCenterPointAnimation.Duration = TimeSpan.FromMilliseconds(450);
            handCenterPointAnimation.Target = "CenterPoint";
            handCenterPointAnimation.InsertExpressionKeyFrame(1, "this.FinalValue");

            var handOffsetAnimation = compositor.CreateVector3KeyFrameAnimation();
            handOffsetAnimation.Duration = TimeSpan.FromMilliseconds(450);
            handOffsetAnimation.Target = "Offset";
            handOffsetAnimation.InsertExpressionKeyFrame(1, "this.FinalValue");

            handImplicitAnimations = compositor.CreateImplicitAnimationCollection();
            handImplicitAnimations["RotationAngleInDegrees"] = handRotationAnimation;
            handImplicitAnimations["Size"] = handSizeAnimation;
            handImplicitAnimations["CenterPoint"] = handCenterPointAnimation;
            handImplicitAnimations["Offset"] = handOffsetAnimation;
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
            UpdateHourVisual(hours < 12);
        }

        private void SetMinutes(int minutes)
        {
            MinutesAngle = minutes * 6;
        }

        private void UpdateTime()
        {
            if (VM != null)
            {
                var isAm = hourHandVisual.Size.Y == hourAmHandOffsetDistance;
                VM.Time = new TimeSpan(HoursFromAngle(HoursAngle) + (isAm ? 0 : 12), MinutesFromAngle(MinutesAngle), 0);
            }
        }


        private void RootGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var distance = GetDistance(e.GetPosition(RootCanvas), RootCanvas.RenderSize);
            var unit = GetUnit(distance);
            if (unit != null)
            {
                var angle = GetAngle(e.GetPosition(RootCanvas), RootCanvas.RenderSize);
                CompleteManipulation((Unit)unit, angle, distance);
            }
        }

        private void RootGrid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            isManipulating = true;

            var distance = GetDistance(e.Position, RootCanvas.RenderSize);
            manipulationUnit = GetUnit(distance);
            if (manipulationUnit == null)
            {
                return;
            }
            DisableImplicitAnimations();
        }

        private void RootGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            isManipulating = false;

            if (manipulationUnit == null)
            {
                return;
            }
            var angle = GetAngle(e.Position, RootCanvas.RenderSize);
            var distance = GetDistance(e.Position, RootCanvas.RenderSize);
            CompleteManipulation((Unit)manipulationUnit, angle, distance);
        }

        private void RootGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (manipulationUnit == null)
            {
                return;
            }

            var angle = GetAngle(e.Position, RootCanvas.RenderSize);
            if (manipulationUnit == Unit.Hours)
            {
                HoursAngle = angle;
                UpdateHourVisual(GetDistance(e.Position, RootCanvas.RenderSize) < hourPmInnerRadius);
            }
            else if (manipulationUnit == Unit.Minutes)
            {
                MinutesAngle = angle;
            }
        }


        private void CompleteManipulation(Unit unit, float angle, float distance)
        {
            EnableImplicitAnimations();

            if (unit == Unit.Hours)
            {
                HoursAngle = RoundHourAngle(angle);
                UpdateHourVisual(distance < hourPmInnerRadius);
            }
            else if (unit == Unit.Minutes)
            {
                MinutesAngle = RoundMinuteAngle(angle);
            }
        }

        private void UpdateHourVisual(bool isAm)
        {
            var length = isAm ? hourAmHandOffsetDistance : hourPmHandOffsetDistance;

            hourHandVisual.Size = new Vector2(4, length);
            hourHandVisual.CenterPoint = new Vector3(hourHandVisual.Size.X / 2, hourHandVisual.Size.Y, 0);
            hourHandVisual.Offset = new Vector3(((float)RootCanvas.Width / 2) - (hourHandVisual.Size.X / 2), ((float)RootCanvas.Height / 2) - hourHandVisual.Size.Y, 0);

            hourIndicatorVisual.CenterPoint = new Vector3((float)HourIndicator.Width / 2, (float)HourIndicator.Height + length, 0);
            hourIndicatorVisual.Offset = new Vector3(((float)RootCanvas.Width / 2) - ((float)HourIndicator.Width / 2), ((float)RootCanvas.Height / 2) - (float)HourIndicator.Height - length, 0);
        }


        private void DisableImplicitAnimations()
        {
            hourHandVisual.ImplicitAnimations = null;
            hourIndicatorVisual.ImplicitAnimations = null;

            minuteHandVisual.ImplicitAnimations = null;
            minuteIndicatorVisual.ImplicitAnimations = null;
        }

        private void EnableImplicitAnimations()
        {
            hourHandVisual.ImplicitAnimations = handImplicitAnimations;
            hourIndicatorVisual.ImplicitAnimations = handImplicitAnimations;

            minuteHandVisual.ImplicitAnimations = handImplicitAnimations;
            minuteIndicatorVisual.ImplicitAnimations = handImplicitAnimations;
        }



        private Unit? GetUnit(float distance)
        {
            if (distance < hourAmInnerRadius) return null;
            if (distance < hourRadius) return Unit.Hours;
            if (distance < minuteRadius) return Unit.Minutes;
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

        private float FixAngle(float currentAngle, float targetAngle, Unit unit)
        {
            if (!isManipulating)
            {
                if (Math.Abs(targetAngle - currentAngle) > 180)
                {
                    if (targetAngle > currentAngle)
                    {
                        targetAngle -= 360;
                    }
                    else
                    {
                        targetAngle += 360;
                    }
                }
                return targetAngle;
            }
            else
            {
                switch (unit)
                {
                case Unit.Hours: return RoundHourAngle(targetAngle);
                case Unit.Minutes: return RoundMinuteAngle(targetAngle);
                default: return targetAngle;
                }
            }
        }

        private float RoundHourAngle(float angle)
        {
            return HoursFromAngle(angle) * 30;
        }

        private float RoundMinuteAngle(float angle)
        {
            return MinutesFromAngle(angle) * 6;
        }

        private int HoursFromAngle(float angle)
        {
            return ((int)Math.Round(NormalizeAngle(angle) / 30d)) % 12;
        }

        private int MinutesFromAngle(float angle)
        {
            return (int)Math.Round(NormalizeAngle(angle) / 6d);
        }

        private float NormalizeAngle(float angle)
        {
            angle = angle % 360;
            if (angle < 0)
            {
                angle += 360;
            }
            return angle;
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
