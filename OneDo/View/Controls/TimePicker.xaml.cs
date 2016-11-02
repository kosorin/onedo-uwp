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

        public float MinutesAngle
        {
            get { return minuteIndicatorVisual.RotationAngleInDegrees; }
            set
            {
                var angle = FixAngle(MinutesAngle, value, Unit.Minutes);
                minuteIndicatorVisual.RotationAngleInDegrees = angle;
                UpdateVisualsOpacity(value, minuteVisuals);
            }
        }

        public float HoursAngle
        {
            get { return hourIndicatorVisual.RotationAngleInDegrees; }
            set
            {
                var angle = FixAngle(HoursAngle, value, Unit.Hours);
                hourIndicatorVisual.RotationAngleInDegrees = angle;
                UpdateVisualsOpacity(value, hourVisuals);
            }
        }

        private void UpdateVisualsOpacity(float value, Dictionary<float, SpriteVisual> collection)
        {
            foreach (var item in collection)
            {
                item.Value.Opacity = value >= item.Key ? 1f : 0.1f;
            }
        }

        private const float size = 150;
        private const float unitSize = 48;

        private const float minuteRadius = size;
        private const float hourRadius = size - unitSize;
        private const float hourInnerRadius = hourRadius - unitSize;

        private const float minuteDistance = size - unitSize;
        private const float hourDistance = minuteDistance - unitSize;

        private Color tickColor;
        private Color highlightedTickColor;

        private ContainerVisual containerVisual;
        private Visual minuteIndicatorVisual;
        private Visual hourIndicatorVisual;
        private Dictionary<float, SpriteVisual> minuteVisuals = new Dictionary<float, SpriteVisual>();
        private Dictionary<float, SpriteVisual> hourVisuals = new Dictionary<float, SpriteVisual>();
        private ImplicitAnimationCollection implicitAnimations;
        private ExpressionAnimation expressionAnimation;

        private bool isManipulating = false;
        private Unit? manipulationUnit = null;

        public TimePicker()
        {
            InitializeComponent();
            InitializeColors();
            InitializeCompositionAnimations();
            InitializeComposition();

            MinutesAngle = 0;
            HoursAngle = 0;
        }

        private void InitializeColors()
        {
            tickColor = (Color)Resources["SystemListLowColor"];
            highlightedTickColor = (Color)Resources["SystemAccentColor"];
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

            implicitAnimations = compositor.CreateImplicitAnimationCollection();
            implicitAnimations["RotationAngleInDegrees"] = handRotationAnimation;
            implicitAnimations["Size"] = handSizeAnimation;
            implicitAnimations["CenterPoint"] = handCenterPointAnimation;
            implicitAnimations["Offset"] = handOffsetAnimation;

            expressionAnimation = compositor.CreateExpressionAnimation();
            expressionAnimation.Expression = $"((visual.RotationAngleInDegrees + 360) % 360) >= angle ? highlighted : normal";
        }

        private void InitializeComposition()
        {
            minuteIndicatorVisual = ElementCompositionPreview.GetElementVisual(MinuteIndicator);
            minuteIndicatorVisual.CenterPoint = new Vector3((float)MinuteIndicator.Width / 2, (float)MinuteIndicator.Height + minuteDistance, 0);
            minuteIndicatorVisual.Offset = new Vector3(((float)RootCanvas.Width / 2) - ((float)MinuteIndicator.Width / 2), ((float)RootCanvas.Height / 2) - (float)MinuteIndicator.Height - minuteDistance, 0);
            minuteIndicatorVisual.Opacity = 0;
            minuteIndicatorVisual.ImplicitAnimations = implicitAnimations;

            hourIndicatorVisual = ElementCompositionPreview.GetElementVisual(HourIndicator);
            hourIndicatorVisual.CenterPoint = new Vector3((float)HourIndicator.Width / 2, (float)HourIndicator.Height + hourDistance, 0);
            hourIndicatorVisual.Offset = new Vector3(((float)RootCanvas.Width / 2) - ((float)HourIndicator.Width / 2), ((float)RootCanvas.Height / 2) - (float)HourIndicator.Height - hourDistance, 0);
            hourIndicatorVisual.Opacity = 0;
            hourIndicatorVisual.ImplicitAnimations = implicitAnimations;


            containerVisual = compositor.CreateContainerVisual();
            CreateTicks(minuteDistance, 5, minuteIndicatorVisual, minuteVisuals);
            CreateTicks(hourDistance, 5, hourIndicatorVisual, hourVisuals);
            ElementCompositionPreview.SetElementChildVisual(RootCanvas, containerVisual);
        }

        private void CreateTicks(float distance, float step, Visual indicatorVisual, Dictionary<float, SpriteVisual> visuals)
        {
            for (float i = 0; i < 60; i++)
            {
                var angle = i * (360 / 60);

                var visual = compositor.CreateSpriteVisual();
                visual.Brush = compositor.CreateColorBrush(highlightedTickColor);
                visual.Size = new Vector2(unitSize / (i % step == 0 ? 3 : 4), 2);
                visual.CenterPoint = new Vector3(0, visual.Size.Y / 2, 0);

                var angleInRadians = DegreesToRadians(angle - 90);
                visual.RotationAngle = angleInRadians;

                var x = ((float)RootCanvas.Width / 2) + ((distance) * (float)Math.Cos(angleInRadians));
                var y = ((float)RootCanvas.Height / 2) + ((distance) * (float)Math.Sin(angleInRadians));
                visual.Offset = new Vector3(x, y, 0);


                expressionAnimation.ClearAllParameters();
                expressionAnimation.SetReferenceParameter("visual", indicatorVisual);
                expressionAnimation.SetScalarParameter("angle", angle);
                expressionAnimation.SetScalarParameter("normal", 0.1f);
                expressionAnimation.SetScalarParameter("highlighted", 1.0f);


                var opacityAnimation = compositor.CreateScalarKeyFrameAnimation();
                opacityAnimation.Duration = TimeSpan.FromMilliseconds(700);
                opacityAnimation.Target = "Opacity";
                opacityAnimation.InsertExpressionKeyFrame(0, "this.StartingValue");
                opacityAnimation.InsertExpressionKeyFrame(1, "this.FinalValue");

                var opacityImplicitAnimations = compositor.CreateImplicitAnimationCollection();
                opacityImplicitAnimations["Opacity"] = opacityAnimation;
                visual.ImplicitAnimations = opacityImplicitAnimations;

                containerVisual.Children.InsertAtTop(visual);
                visuals[angle] = visual;
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
            if (VM != null)
            {
                //var isAm = hourHandVisual.Size.Y == hourAmHandOffsetDistance;
                //VM.Time = new TimeSpan(HoursFromAngle(HoursAngle) + (isAm ? 0 : 12), MinutesFromAngle(MinutesAngle), 0);
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
            }
            else if (unit == Unit.Minutes)
            {
                MinutesAngle = RoundMinuteAngle(angle);
            }
        }

        private void DisableImplicitAnimations()
        {
            hourIndicatorVisual.ImplicitAnimations = null;
            minuteIndicatorVisual.ImplicitAnimations = null;
        }

        private void EnableImplicitAnimations()
        {
            hourIndicatorVisual.ImplicitAnimations = implicitAnimations;
            minuteIndicatorVisual.ImplicitAnimations = implicitAnimations;
        }



        private Unit? GetUnit(float distance)
        {
            if (distance < hourInnerRadius) return null;
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
                return targetAngle;
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

        private float DegreesToRadians(float angle)
        {
            return (angle / 180f) * (float)Math.PI;
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
