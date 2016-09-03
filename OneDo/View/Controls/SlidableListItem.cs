﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace OneDo.View.Controls
{
    [TemplatePart(Name = PART_CONTENT_PANEL, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PART_COMMAND_CONTAINER, Type = typeof(Grid))]
    [TemplatePart(Name = PART_LEFT_COMMAND_PANEL, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = PART_RIGHT_COMMAND_PANEL, Type = typeof(FrameworkElement))]
    public class SlidableListItem : ContentControl
    {
        const string PART_CONTENT_PANEL = "ContentPanel";
        const string PART_COMMAND_CONTAINER = "CommandContainer";
        const string PART_LEFT_COMMAND_PANEL = "LeftCommandPanel";
        const string PART_RIGHT_COMMAND_PANEL = "RightCommandPanel";


        public event EventHandler RightCommandRequested;

        public event EventHandler LeftCommandRequested;


        private FrameworkElement contentPanel;

        private Grid commandContainer;

        private FrameworkElement leftCommandPanel;

        private FrameworkElement rightCommandPanel;

        protected readonly Compositor compositor;

        private readonly CompositionAnimation resetOffsetAnimation;

        private readonly CompositionAnimation resetOpacityAnimation;

        private readonly CompositionAnimation offsetAnimation;

        private readonly CompositionAnimation opacityAnimation;

        public SlidableListItem()
        {
            DefaultStyleKey = typeof(SlidableListItem);

            compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            resetOffsetAnimation = CreateAnimation(200);
            resetOpacityAnimation = CreateAnimation(200);
            offsetAnimation = CreateAnimation(350);
            opacityAnimation = CreateAnimation(350);
        }


        protected override void OnApplyTemplate()
        {
            contentPanel = GetTemplateChild(PART_CONTENT_PANEL) as FrameworkElement;
            commandContainer = GetTemplateChild(PART_COMMAND_CONTAINER) as Grid;
            leftCommandPanel = GetTemplateChild(PART_LEFT_COMMAND_PANEL) as FrameworkElement;
            rightCommandPanel = GetTemplateChild(PART_RIGHT_COMMAND_PANEL) as FrameworkElement;

            contentPanel.ManipulationDelta += ContentGrid_ManipulationDelta;
            contentPanel.ManipulationCompleted += ContentGrid_ManipulationCompleted;

            base.OnApplyTemplate();
        }

        private void ContentGrid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!MouseSlidingEnabled && e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                return;
            }

            var contentVisual = ElementCompositionPreview.GetElementVisual(contentPanel);
            var leftCommandVisual = ElementCompositionPreview.GetElementVisual(leftCommandPanel);
            var rightCommandVisual = ElementCompositionPreview.GetElementVisual(rightCommandPanel);

            AnimateResetOffset(contentVisual);
            AnimateResetOffset(leftCommandVisual);
            AnimateResetOffset(rightCommandVisual);
            AnimateResetOpacity(leftCommandVisual);
            AnimateResetOpacity(rightCommandVisual);

            TryExecuteCommand(contentVisual.Offset.X);
        }

        private void ContentGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!MouseSlidingEnabled && e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                return;
            }

            var contentVisual = ElementCompositionPreview.GetElementVisual(contentPanel);

            var previousOffset = contentVisual.Offset.X;
            SetOffset(contentVisual, previousOffset + (float)e.Delta.Translation.X);
            var offset = contentVisual.Offset.X;

            var previousAbsOffset = Math.Abs(previousOffset);
            var absOffset = Math.Abs(offset);


            var factor = offset > 0 ? 1 : -1;
            var visibleCommandVisual = ElementCompositionPreview.GetElementVisual(offset > 0 ? leftCommandPanel : rightCommandPanel);
            var hiddenCommandVisual = ElementCompositionPreview.GetElementVisual(offset > 0 ? rightCommandPanel : leftCommandPanel);

            commandContainer.Background = offset > 0 ? LeftBackground : RightBackground;

            visibleCommandVisual.Opacity = 1;
            hiddenCommandVisual.Opacity = 0;
            if (previousAbsOffset < ActivationOffset)
            {
                if (absOffset < ActivationOffset)
                {
                    SetOffset(visibleCommandVisual, factor * absOffset / 3);
                }
                else
                {
                    AnimateOffset(visibleCommandVisual, (float)(factor * ActivatedOffset));
                }
            }
        }

        private void SetOffset(Visual visual, float offset)
        {
            visual.Offset = new Vector3(offset, visual.Offset.Y, visual.Offset.Z);
        }

        private void AnimateResetOffset(Visual visual)
        {
            resetOffsetAnimation.SetScalarParameter("To", 0f);
            visual.StartAnimation("Offset.X", resetOffsetAnimation);
        }

        private void AnimateOffset(Visual visual, float offset)
        {
            offsetAnimation.SetScalarParameter("To", offset);
            visual.StartAnimation("Offset.X", offsetAnimation);
        }

        private void AnimateResetOpacity(Visual visual)
        {
            resetOpacityAnimation.SetScalarParameter("To", 0f);
            visual.StartAnimation("Opacity", resetOpacityAnimation);
        }

        private void AnimateOpacity(Visual visual, float opacity)
        {
            opacityAnimation.SetScalarParameter("To", opacity);
            visual.StartAnimation("Opacity", opacityAnimation);
        }

        private CompositionAnimation CreateAnimation(int duration)
        {
            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.Duration = TimeSpan.FromMilliseconds(duration);
            animation.InsertExpressionKeyFrame(0f, "this.CurrentValue");
            animation.InsertExpressionKeyFrame(1f, "To");
            return animation;
        }

        private void TryExecuteCommand(double x)
        {
            if (x < -ActivationOffset)
            {
                RightCommandRequested?.Invoke(this, new EventArgs());
                RightCommand?.Execute(null);
            }
            else if (x > ActivationOffset)
            {
                LeftCommandRequested?.Invoke(this, new EventArgs());
                LeftCommand?.Execute(null);
            }
        }


        public double ActivatedOffset
        {
            get { return (double)GetValue(ActivatedOffsetProperty); }
            set { SetValue(ActivatedOffsetProperty, value); }
        }

        public static readonly DependencyProperty ActivatedOffsetProperty =
            DependencyProperty.Register(nameof(ActivatedOffset), typeof(double), typeof(SlidableListItem), new PropertyMetadata(40f));

        public double ActivationOffset
        {
            get { return (double)GetValue(ActivationOffsetProperty); }
            set { SetValue(ActivationOffsetProperty, value); }
        }

        public static readonly DependencyProperty ActivationOffsetProperty =
            DependencyProperty.Register(nameof(ActivationOffset), typeof(double), typeof(SlidableListItem), new PropertyMetadata(80f));

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