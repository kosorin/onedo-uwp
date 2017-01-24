using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using OneDo.Common.UI;
using OneDo.Services.InfoService;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using System.Windows.Input;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace OneDo.View.Controls
{
    [TemplatePart(Name = RootGridPartName, Type = typeof(Grid))]
    public class InfoBar : Control
    {
        private const string RootGridPartName = "PART_RootGrid";

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(InfoBar), new PropertyMetadata(""));

        public bool IsActionVisible
        {
            get { return (bool)GetValue(IsActionVisibleProperty); }
            set { SetValue(IsActionVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsActionVisibleProperty =
            DependencyProperty.Register(nameof(IsActionVisible), typeof(bool), typeof(InfoBar), new PropertyMetadata(true));

        public string ActionGlyph
        {
            get { return (string)GetValue(ActionGlyphProperty); }
            set { SetValue(ActionGlyphProperty, value); }
        }

        public static readonly DependencyProperty ActionGlyphProperty =
            DependencyProperty.Register(nameof(ActionGlyph), typeof(string), typeof(InfoBar), new PropertyMetadata(null));

        public string ActionText
        {
            get { return (string)GetValue(ActionTextProperty); }
            set { SetValue(ActionTextProperty, value); }
        }

        public static readonly DependencyProperty ActionTextProperty =
            DependencyProperty.Register(nameof(ActionText), typeof(string), typeof(InfoBar), new PropertyMetadata(null));

        public ICommand ActionCommand
        {
            get { return (ICommand)GetValue(ActionCommandProperty); }
            set { SetValue(ActionCommandProperty, value); }
        }

        public static readonly DependencyProperty ActionCommandProperty =
            DependencyProperty.Register(nameof(ActionCommand), typeof(ICommand), typeof(InfoBar), new PropertyMetadata(null));

        public object ActionCommandParameter
        {
            get { return (object)GetValue(ActionCommandParameterProperty); }
            set { SetValue(ActionCommandParameterProperty, value); }
        }

        public static readonly DependencyProperty ActionCommandParameterProperty =
            DependencyProperty.Register(nameof(ActionCommandParameter), typeof(object), typeof(InfoBar), new PropertyMetadata(null));

        private Grid rootGrid;

        private Compositor compositor;

        private Visual visual;

        private List<AnimationInfo> showAnimationInfos;

        private List<AnimationInfo> hideAnimationInfos;

        private Timer timer;

        private Brush defaultBackground;

        private int counter = 0;

        public InfoBar()
        {
            DefaultStyleKey = typeof(InfoBar);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            rootGrid = GetTemplateChild(RootGridPartName) as Grid;
            InitializeComposition();
            Visibility = Visibility.Collapsed;
        }

        private void InitializeComposition()
        {
            visual = ElementCompositionPreview.GetElementVisual(rootGrid);
            visual.Opacity = 0f;
            visual.Offset = new Vector3(0, 48, 0);

            compositor = visual.Compositor;

            defaultBackground = (Brush)Resources["SystemControlBackgroundAccentBrush"];

            var opacityShowAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityShowAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            opacityShowAnimation.InsertKeyFrame(0.3f, 0);
            opacityShowAnimation.InsertKeyFrame(1, 1);

            var offsetShowAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetShowAnimation.Duration = TimeSpan.FromMilliseconds(1000);
            offsetShowAnimation.InsertKeyFrame(0.3f, 48);
            offsetShowAnimation.InsertKeyFrame(1, 0);

            showAnimationInfos = new List<AnimationInfo>
            {
                new AnimationInfo("Opacity", opacityShowAnimation),
                new AnimationInfo("Offset.Y", offsetShowAnimation),
            };

            var opacityHideAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityHideAnimation.Duration = TimeSpan.FromMilliseconds(450);
            opacityHideAnimation.InsertKeyFrame(0, 1);
            opacityHideAnimation.InsertKeyFrame(1, 0);

            var offsetHideAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetHideAnimation.Duration = TimeSpan.FromMilliseconds(450);
            offsetHideAnimation.InsertKeyFrame(0, 0);
            offsetHideAnimation.InsertKeyFrame(1, 48);

            hideAnimationInfos = new List<AnimationInfo>
            {
                new AnimationInfo("Opacity", opacityHideAnimation),
                new AnimationInfo("Offset.Y", offsetHideAnimation),
            };
        }

        public void Show(InfoMessage message)
        {
            if (message == null)
            {
                Hide();
                return;
            }

            ApplyMessage(message);

            if (counter == 0)
            {
                Interlocked.Increment(ref counter);
            }
            Visibility = Visibility.Visible;
            foreach (var animationInfo in showAnimationInfos)
            {
                visual.StartAnimation(animationInfo.PropertyName, animationInfo.Animation);
            }

            StartTimer((int)message.Duration.TotalMilliseconds);
        }

        public void Hide()
        {
            StopTimer();

            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            batch.Completed += (s, e) =>
            {
                if (counter > 0)
                {
                    Interlocked.Decrement(ref counter);
                }
                if (counter == 0)
                {
                    Visibility = Visibility.Collapsed;
                }
            };
            foreach (var animationInfo in hideAnimationInfos)
            {
                visual.StartAnimation(animationInfo.PropertyName, animationInfo.Animation);
            }
            batch.End();
        }

        private void ApplyMessage(InfoMessage message)
        {
            Text = message.Text;
            IsActionVisible = message.Action != null;
            Background = message.Color != null
                ? new SolidColorBrush(message.Color.ToColor())
                : defaultBackground;
            if (IsActionVisible)
            {
                ActionGlyph = message.Action.Glyph;
                ActionText = message.Action.Text;
                ActionCommand = new AsyncRelayCommand(async () =>
                {
                    await message.Action.Action();
                    Hide();
                });
                ActionCommandParameter = null;
            }
        }


        private void StartTimer(int dueTime)
        {
            if (timer == null)
            {
                timer = new Timer(TimerCallback, null, dueTime, Timeout.Infinite);
            }
            else
            {
                timer.Change(dueTime, Timeout.Infinite);
            }
        }

        private void StopTimer()
        {
            timer?.Dispose();
            timer = null;
        }

        private void TimerCallback(object state)
        {
            Hide();
        }
    }
}
