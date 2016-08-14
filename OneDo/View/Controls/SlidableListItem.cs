using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace OneDo.View.Controls
{
    [TemplatePart(Name = PART_CONTENT_GRID, Type = typeof(Grid))]
    [TemplatePart(Name = PART_COMMAND_CONTAINER, Type = typeof(Grid))]
    [TemplatePart(Name = PART_LEFT_COMMAND_PANEL, Type = typeof(StackPanel))]
    [TemplatePart(Name = PART_RIGHT_COMMAND_PANEL, Type = typeof(StackPanel))]
    public class SlidableListItem : ContentControl
    {
        const string PART_CONTENT_GRID = "ContentGrid";
        const string PART_COMMAND_CONTAINER = "CommandContainer";
        const string PART_LEFT_COMMAND_PANEL = "LeftCommandPanel";
        const string PART_RIGHT_COMMAND_PANEL = "RightCommandPanel";

        // Content Container
        private Grid contentGrid;

        // transform for sliding content
        private CompositeTransform transform;

        // container for command content
        private Grid commandContainer;

        // container for left command content
        private StackPanel leftCommandPanel;

        // transform for left command content
        private CompositeTransform leftCommandTransform;

        // container for right command content
        private StackPanel rightCommandPanel;

        // transform for right command content
        private CompositeTransform rightCommandTransform;

        // doubleanimation for snaping back to default position
        private DoubleAnimation contentAnimation;

        // storyboard for snaping back to default position
        private Storyboard contentStoryboard;


        public event EventHandler RightCommandRequested;

        public event EventHandler LeftCommandRequested;


        public SlidableListItem()
        {
            DefaultStyleKey = typeof(SlidableListItem);
        }


        protected override void OnApplyTemplate()
        {
            contentGrid = this.GetTemplateChild(PART_CONTENT_GRID) as Grid;
            commandContainer = this.GetTemplateChild(PART_COMMAND_CONTAINER) as Grid;
            leftCommandPanel = this.GetTemplateChild(PART_LEFT_COMMAND_PANEL) as StackPanel;
            rightCommandPanel = this.GetTemplateChild(PART_RIGHT_COMMAND_PANEL) as StackPanel;

            transform = contentGrid.RenderTransform as CompositeTransform;

            leftCommandTransform = leftCommandPanel.RenderTransform as CompositeTransform;
            rightCommandTransform = rightCommandPanel.RenderTransform as CompositeTransform;

            contentGrid.ManipulationDelta += ContentGrid_ManipulationDelta;
            contentGrid.ManipulationCompleted += ContentGrid_ManipulationCompleted;

            contentAnimation = new DoubleAnimation();
            Storyboard.SetTarget(contentAnimation, transform);
            Storyboard.SetTargetProperty(contentAnimation, nameof(transform.TranslateX));
            contentAnimation.To = 0;
            contentAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));

            contentStoryboard = new Storyboard();
            contentStoryboard.Children.Add(contentAnimation);

            commandContainer.Background = LeftBackground as SolidColorBrush;

            base.OnApplyTemplate();
        }

        private void ContentGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!MouseSlidingEnabled && e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
                return;

            var x = transform.TranslateX;
            contentAnimation.From = x;
            contentStoryboard.Begin();

            leftCommandTransform.TranslateX = 0;
            rightCommandTransform.TranslateX = 0;
            leftCommandPanel.Opacity = 1;
            rightCommandPanel.Opacity = 1;

            if (x < -ActivationWidth)
            {
                RightCommandRequested?.Invoke(this, new EventArgs());
                RightCommand?.Execute(null);
            }
            else if (x > ActivationWidth)
            {
                LeftCommandRequested?.Invoke(this, new EventArgs());
                LeftCommand?.Execute(null);
            }
        }

        private void ContentGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!MouseSlidingEnabled && e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
                return;

            transform.TranslateX += e.Delta.Translation.X;
            var abs = Math.Abs(transform.TranslateX);

            if (transform.TranslateX > 0)
            {
                commandContainer.Background = LeftBackground as SolidColorBrush;

                leftCommandPanel.Opacity = 1;
                rightCommandPanel.Opacity = 0;

                if (abs < ActivationWidth)
                    leftCommandTransform.TranslateX = transform.TranslateX / 2;
                else
                    leftCommandTransform.TranslateX = 16;
            }
            else
            {
                commandContainer.Background = RightBackground as SolidColorBrush;

                rightCommandPanel.Opacity = 1;
                leftCommandPanel.Opacity = 0;

                if (abs < ActivationWidth)
                    rightCommandTransform.TranslateX = transform.TranslateX / 2;
                else
                    rightCommandTransform.TranslateX = -16;
            }

        }



        public double ActivationWidth
        {
            get { return (double)GetValue(ActivationWidthProperty); }
            set { SetValue(ActivationWidthProperty, value); }
        }

        public static readonly DependencyProperty ActivationWidthProperty =
            DependencyProperty.Register(nameof(ActivationWidth), typeof(double), typeof(SlidableListItem), new PropertyMetadata(80));

        public string LeftGlyph
        {
            get { return (string)GetValue(LeftGlyphProperty); }
            set { SetValue(LeftGlyphProperty, value); }
        }

        public static readonly DependencyProperty LeftGlyphProperty =
            DependencyProperty.Register(nameof(LeftGlyph), typeof(string), typeof(SlidableListItem), new PropertyMetadata("X"));

        public string RightGlyph
        {
            get { return (string)GetValue(RightGlyphProperty); }
            set { SetValue(RightGlyphProperty, value); }
        }

        public static readonly DependencyProperty RightGlyphProperty =
            DependencyProperty.Register(nameof(RightGlyph), typeof(string), typeof(SlidableListItem), new PropertyMetadata("Y"));

        public string LeftLabel
        {
            get { return (string)GetValue(LeftLabelProperty); }
            set { SetValue(LeftLabelProperty, value); }
        }

        public static readonly DependencyProperty LeftLabelProperty =
            DependencyProperty.Register(nameof(LeftLabel), typeof(string), typeof(SlidableListItem), new PropertyMetadata(""));

        public string RightLabel
        {
            get { return (string)GetValue(RightLabelProperty); }
            set { SetValue(RightLabelProperty, value); }
        }

        public static readonly DependencyProperty RightLabelProperty =
            DependencyProperty.Register(nameof(RightLabel), typeof(string), typeof(SlidableListItem), new PropertyMetadata(""));

        public Brush LeftForeground
        {
            get { return (Brush)GetValue(LeftForegroundProperty); }
            set { SetValue(LeftForegroundProperty, value); }
        }

        public static readonly DependencyProperty LeftForegroundProperty =
            DependencyProperty.Register(nameof(LeftForeground), typeof(Brush), typeof(SlidableListItem), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush RightForeground
        {
            get { return (Brush)GetValue(RightForegroundProperty); }
            set { SetValue(RightForegroundProperty, value); }
        }

        public static readonly DependencyProperty RightForegroundProperty =
            DependencyProperty.Register(nameof(RightForeground), typeof(Brush), typeof(SlidableListItem), new PropertyMetadata(new SolidColorBrush(Colors.White)));

        public Brush LeftBackground
        {
            get { return (Brush)GetValue(LeftBackgroundProperty); }
            set { SetValue(LeftBackgroundProperty, value); }
        }

        public static readonly DependencyProperty LeftBackgroundProperty =
            DependencyProperty.Register(nameof(LeftBackground), typeof(Brush), typeof(SlidableListItem), new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public Brush RightBackground
        {
            get { return (Brush)GetValue(RightBackgroundProperty); }
            set { SetValue(RightBackgroundProperty, value); }
        }

        public static readonly DependencyProperty RightBackgroundProperty =
            DependencyProperty.Register(nameof(RightBackground), typeof(Brush), typeof(SlidableListItem), new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public bool MouseSlidingEnabled
        {
            get { return (bool)GetValue(MouseSlidingEnabledProperty); }
            set { SetValue(MouseSlidingEnabledProperty, value); }
        }

        public static readonly DependencyProperty MouseSlidingEnabledProperty =
            DependencyProperty.Register(nameof(MouseSlidingEnabled), typeof(bool), typeof(SlidableListItem), new PropertyMetadata(false));

        public ICommand LeftCommand
        {
            get { return (ICommand)GetValue(LeftCommandProperty); }
            set { SetValue(LeftCommandProperty, value); }
        }

        public static readonly DependencyProperty LeftCommandProperty =
            DependencyProperty.Register(nameof(LeftCommand), typeof(ICommand), typeof(SlidableListItem), new PropertyMetadata(null));

        public ICommand RightCommand
        {
            get { return (ICommand)GetValue(RightCommandProperty); }
            set { SetValue(RightCommandProperty, value); }
        }

        public static readonly DependencyProperty RightCommandProperty =
            DependencyProperty.Register(nameof(RightCommand), typeof(ICommand), typeof(SlidableListItem), new PropertyMetadata(null));
    }
}