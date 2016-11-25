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
            Messenger.Default.Register<InfoMessage>(this, InfoBar.Show);
        }

#if DEBUG
        private void InitializeDebug()
        {
            var debugButton = new AppBarButton
            {
                Label = "Debug",
            };
            debugButton.Tapped += (s, e) => VM.UIHost.ModalService.Show(new DebugViewModel(VM.UIHost.ProgressService));

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

            MainCommandBar.SecondaryCommands.Insert(0, new AppBarSeparator());
            MainCommandBar.SecondaryCommands.Insert(0, resetDataButton);
            MainCommandBar.SecondaryCommands.Insert(0, switchRequestedThemeButton);
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
    }
}
