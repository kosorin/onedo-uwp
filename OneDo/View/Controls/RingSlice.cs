using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace OneDo.View.Controls
{
    public class RingSlice : Path
    {
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty) % 360; }
            set { SetValue(StartAngleProperty, value % 360); }
        }

        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(RingSlice), new PropertyMetadata(0d, StartAngle_Changed));

        private static void StartAngle_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldStartAngle = (double)e.OldValue;
            var newStartAngle = (double)e.NewValue;
            target.OnStartAngleChanged(oldStartAngle, newStartAngle);
        }

        private void OnStartAngleChanged(double oldStartAngle, double newStartAngle)
        {
            UpdatePath();
        }


        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(RingSlice), new PropertyMetadata(0d, EndAngle_Changed));


        private static void EndAngle_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldEndAngle = (double)e.OldValue;
            var newEndAngle = (double)e.NewValue;
            target.OnEndAngleChanged(oldEndAngle, newEndAngle);
        }

        private void OnEndAngleChanged(double oldEndAngle, double newEndAngle)
        {
            UpdatePath();
        }


        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(nameof(Radius), typeof(double), typeof(RingSlice), new PropertyMetadata(0d, Radius_Changed));

        private static void Radius_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldRadius = (double)e.OldValue;
            var newRadius = (double)e.NewValue;
            target.OnRadiusChanged(oldRadius, newRadius);
        }

        private void OnRadiusChanged(double oldRadius, double newRadius)
        {
            Width = Height = 2 * Radius;
            UpdatePath();
        }


        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }

        public static readonly DependencyProperty InnerRadiusProperty =
            DependencyProperty.Register(nameof(InnerRadius), typeof(double), typeof(RingSlice), new PropertyMetadata(0d, InnerRadius_Changed));

        private static void InnerRadius_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)sender;
            var oldInnerRadius = (double)e.OldValue;
            var newInnerRadius = (double)e.NewValue;
            target.OnInnerRadiusChanged(oldInnerRadius, newInnerRadius);
        }

        private void OnInnerRadiusChanged(double oldInnerRadius, double newInnerRadius)
        {
            if (newInnerRadius < 0)
            {
                throw new ArgumentException($"{nameof(InnerRadius)} can't be a negative value", nameof(InnerRadius));
            }

            UpdatePath();
        }


        public Point? Center
        {
            get { return (Point?)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register(nameof(Center), typeof(Point?), typeof(RingSlice), new PropertyMetadata(null, Center_Changed));

        private static void Center_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = (RingSlice)d;
            var oldCenter = (Point?)e.OldValue;
            var newCenter = target.Center;
            target.OnCenterChanged(oldCenter, newCenter);
        }

        private void OnCenterChanged(Point? oldCenter, Point? newCenter)
        {
            UpdatePath();
        }


        private bool isUpdating = false;

        public RingSlice()
        {
            SizeChanged += OnSizeChanged;
        }

        private void OnStrokeThicknessChanged(object sender, double e)
        {
            UpdatePath();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            UpdatePath();
        }


        public void BeginUpdate()
        {
            isUpdating = true;
        }

        public void EndUpdate()
        {
            isUpdating = false;
            UpdatePath();
        }

        private void UpdatePath()
        {
            if (isUpdating)
            {
                return;
            }

            var innerRadius = InnerRadius + StrokeThickness / 2;
            var outerRadius = Radius - StrokeThickness / 2;

            if (ActualWidth == 0 || innerRadius <= 0 || outerRadius < innerRadius)
            {
                return;
            }

            var pathGeometry = new PathGeometry();
            var pathFigure = new PathFigure();
            pathFigure.IsClosed = true;

            var center = Center ?? new Point(
                outerRadius + StrokeThickness / 2,
                outerRadius + StrokeThickness / 2);

            // Starting Point
            pathFigure.StartPoint = new Point(
                center.X + Math.Sin(StartAngle * Math.PI / 180) * innerRadius,
                center.Y - Math.Cos(StartAngle * Math.PI / 180) * innerRadius);

            // Inner Arc
            var innerArcSegment = new ArcSegment();
            innerArcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
            innerArcSegment.Point = new Point(
                center.X + Math.Sin(EndAngle * Math.PI / 180) * innerRadius,
                center.Y - Math.Cos(EndAngle * Math.PI / 180) * innerRadius);
            innerArcSegment.Size = new Size(innerRadius, innerRadius);
            innerArcSegment.SweepDirection = SweepDirection.Clockwise;

            var lineSegment = new LineSegment
            {
                Point = new Point(
                    center.X + Math.Sin(EndAngle * Math.PI / 180) * outerRadius,
                    center.Y - Math.Cos(EndAngle * Math.PI / 180) * outerRadius)
            };

            // Outer Arc
            var outerArcSegment = new ArcSegment();
            outerArcSegment.IsLargeArc = (EndAngle - StartAngle) >= 180.0;
            outerArcSegment.Point = new Point(
                center.X + Math.Sin(StartAngle * Math.PI / 180) * outerRadius,
                center.Y - Math.Cos(StartAngle * Math.PI / 180) * outerRadius);
            outerArcSegment.Size = new Size(outerRadius, outerRadius);
            outerArcSegment.SweepDirection = SweepDirection.Counterclockwise;

            pathFigure.Segments.Add(innerArcSegment);
            pathFigure.Segments.Add(lineSegment);
            pathFigure.Segments.Add(outerArcSegment);
            pathGeometry.Figures.Add(pathFigure);
            InvalidateArrange();
            Data = pathGeometry;
        }
    }
}
