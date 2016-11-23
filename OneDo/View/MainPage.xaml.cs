using System;
using OneDo.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;
using OneDo.ViewModel.Args;
using OneDo.Services.InfoService;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml.Media;
using OneDo.Common.Media;
using OneDo.ViewModel.Commands;

namespace OneDo.View
{
    public sealed partial class MainPage : ExtendedPage, IXBind<MainViewModel>
    {
        public MainViewModel VM => (MainViewModel)ViewModel;

        private Visual infoBarContainerVisual;

        private List<AnimationInfo> infoBarContainerInAnimationInfos;

        private List<AnimationInfo> infoBarContainerOutAnimationInfos;

        private Timer infoBarTimer;

        private Brush defaultInfoBarBackground;

        public MainPage()
        {
            InitializeComponent();
            InitializeModalAnimations();
            InitializeInfoBar();

#if DEBUG
            InitializeDebug();
#endif
        }

        private void InitializeModalAnimations()
        {
            var noteEditorFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            noteEditorFadeInAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            noteEditorFadeInAnimation.InsertExpressionKeyFrame(0, "Height");
            noteEditorFadeInAnimation.InsertKeyFrame(1, 0, ModalContainer.DefaultEasing);
            ModalContainer.AddFadeInAnimation<NoteEditorViewModel>("Offset.Y", noteEditorFadeInAnimation);

            var noteEditorFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            noteEditorFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            noteEditorFadeOutAnimation.InsertKeyFrame(0, 0);
            noteEditorFadeOutAnimation.InsertExpressionKeyFrame(1, "Height", ModalContainer.DefaultEasing);
            ModalContainer.AddFadeOutAnimation<NoteEditorViewModel>("Offset.Y", noteEditorFadeOutAnimation);

            var settingsFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            settingsFadeInAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            settingsFadeInAnimation.InsertExpressionKeyFrame(0, "Width");
            settingsFadeInAnimation.InsertKeyFrame(1, 0, ModalContainer.DefaultEasing);
            ModalContainer.AddFadeInAnimation<SettingsViewModel>("Offset.X", settingsFadeInAnimation);

            var settingsFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            settingsFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            settingsFadeOutAnimation.InsertKeyFrame(0, 0);
            settingsFadeOutAnimation.InsertExpressionKeyFrame(1, "Width", ModalContainer.DefaultEasing);
            ModalContainer.AddFadeOutAnimation<SettingsViewModel>("Offset.X", settingsFadeOutAnimation);
        }

        private void InitializeInfoBar()
        {
            defaultInfoBarBackground = (Brush)Resources["SystemControlBackgroundAccentBrush"];

            var opacityInAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityInAnimation.Duration = TimeSpan.FromMilliseconds(750);
            opacityInAnimation.InsertKeyFrame(0, 0);
            opacityInAnimation.InsertKeyFrame(1, 1);

            var offsetInAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetInAnimation.Duration = TimeSpan.FromMilliseconds(750);
            offsetInAnimation.InsertKeyFrame(0, 48);
            offsetInAnimation.InsertKeyFrame(1, 0);

            infoBarContainerInAnimationInfos = new List<AnimationInfo>
            {
                new AnimationInfo("Opacity", opacityInAnimation),
                new AnimationInfo("Offset.Y", offsetInAnimation),
            };

            var opacityOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            opacityOutAnimation.Duration = TimeSpan.FromMilliseconds(450);
            opacityOutAnimation.InsertKeyFrame(0, 1);
            opacityOutAnimation.InsertKeyFrame(1, 0);

            var offsetOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            offsetOutAnimation.Duration = TimeSpan.FromMilliseconds(450);
            offsetOutAnimation.InsertKeyFrame(0, 0);
            offsetOutAnimation.InsertKeyFrame(1, 48);

            infoBarContainerOutAnimationInfos = new List<AnimationInfo>
            {
                new AnimationInfo("Opacity", opacityOutAnimation),
                new AnimationInfo("Offset.Y", offsetOutAnimation),
            };

            infoBarContainerVisual = ElementCompositionPreview.GetElementVisual(InfoBarContainer);

            Messenger.Default.Register<InfoMessage>(this, OnInfoMessage);
        }

#if DEBUG
        private void InitializeDebug()
        {
            var debugButton = new AppBarButton
            {
                Label = "Debug",
            };
            debugButton.Tapped += (s, e) => VM.ModalService.Show(new DebugViewModel(VM.ProgressService));

            var switchRequestedThemeButton = new AppBarButton
            {
                Label = "Switch RequestedTheme",
            };
            switchRequestedThemeButton.Tapped += SwitchRequestedTheme_Tapped;

            var resetDataButton = new AppBarButton
            {
                Label = "Reset",
            };
            resetDataButton.Tapped += async (s, e) => await VM.ResetData();

            var bandButton = new AppBarButton
            {
                Label = "Band",
            };
            bandButton.Tapped += async (s, e) => await VM.Band();

            MainCommandBar.SecondaryCommands.Insert(0, new AppBarSeparator());
            //MainCommandBar.SecondaryCommands.Insert(0, bandButton);
            MainCommandBar.SecondaryCommands.Insert(0, resetDataButton);
            //MainCommandBar.SecondaryCommands.Insert(0, switchRequestedThemeButton);
            MainCommandBar.SecondaryCommands.Insert(0, debugButton);
        }

        private void SwitchRequestedTheme_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var targetTheme = RequestedTheme;
            if (targetTheme == ElementTheme.Default)
            {
                switch (Application.Current.RequestedTheme)
                {
                case ApplicationTheme.Light: targetTheme = ElementTheme.Light; break;
                case ApplicationTheme.Dark: targetTheme = ElementTheme.Dark; break;
                }
            }
            switch (targetTheme)
            {
            case ElementTheme.Light: targetTheme = ElementTheme.Dark; break;
            case ElementTheme.Dark: targetTheme = ElementTheme.Light; break;
            }
            RequestedTheme = targetTheme;
        }
#endif

        private void OnInfoMessage(InfoMessage message)
        {
            if (message != null)
            {
                SetInfoBar(message);

                var timerDueTime = (int)message.Duration.TotalMilliseconds;
                if (infoBarTimer == null)
                {
                    ShowInfoBar();
                    infoBarTimer = new Timer(InfoBarTimerCallback, null, timerDueTime, Timeout.Infinite);
                }
                else
                {
                    infoBarTimer.Change(timerDueTime, Timeout.Infinite);
                }
            }
            else
            {
                HideInfoBar();
            }
        }

        private void InfoBarTimerCallback(object state)
        {
            HideInfoBar();
        }

        private void SetInfoBar(InfoMessage message)
        {
            InfoBar.Text = message.Text;
            InfoBar.IsActionVisible = message.Action != null;
            InfoBar.Background = message.Color != null
                ? new SolidColorBrush(ColorHelper.FromHex(message.Color))
                : defaultInfoBarBackground;
            if (InfoBar.IsActionVisible)
            {
                InfoBar.ActionGlyph = message.Action.Glyph;
                InfoBar.ActionText = message.Action.Text;
                InfoBar.ActionCommand = new AsyncRelayCommand(async () =>
                {
                    await message.Action.Action();
                    HideInfoBar();
                });
                InfoBar.ActionCommandParameter = null;
            }
        }

        private void HideInfoBar()
        {
            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            batch.Completed += (s, e) =>
            {
                InfoBar.Visibility = Visibility.Collapsed;
            };
            foreach (var animationInfo in infoBarContainerOutAnimationInfos)
            {
                infoBarContainerVisual.StartAnimation(animationInfo.PropertyName, animationInfo.Animation);
            }
            batch.End();

            infoBarTimer?.Dispose();
            infoBarTimer = null;
        }

        private void ShowInfoBar()
        {
            InfoBar.Visibility = Visibility.Visible;

            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            batch.Completed += (s, e) =>
            {
                InfoBar.Visibility = Visibility.Visible;
            };
            foreach (var animationInfo in infoBarContainerInAnimationInfos)
            {
                infoBarContainerVisual.StartAnimation(animationInfo.PropertyName, animationInfo.Animation);
            }
            batch.End();
        }
    }
}
