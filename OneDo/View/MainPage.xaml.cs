﻿using System;
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
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Linq;

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
            InsertMenuSeparator();
            InsertMenuButtonAsync("Reset", VM.ResetData);
            InsertMenuButton("Switch RequestedTheme", SwitchRequestedTheme);
            InsertMenuButton("Remove all from schedule", VM.ToastService.RemoveAllFromSchedule);
            InsertMenuButtonAsync("Show schedule", ShowSchedule);
            InsertMenuButton("Debug", () => VM.UIHost.ModalService.Show(new DebugViewModel(VM.UIHost.ProgressService)));
        }

        private void InsertMenuSeparator()
        {
            MainCommandBar.SecondaryCommands.Insert(0, new AppBarSeparator());
        }

        private void InsertMenuButton(string label, Action action)
        {
            var button = new AppBarButton
            {
                Label = label,
            };
            button.Tapped += (s, e) => action();
            MainCommandBar.SecondaryCommands.Insert(0, button);
        }

        private void InsertMenuButtonAsync(string label, Func<Task> action)
        {
            var button = new AppBarButton
            {
                Label = label,
            };
            button.Tapped += async (s, e) => await action();
            MainCommandBar.SecondaryCommands.Insert(0, button);
        }

        private void SwitchRequestedTheme()
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

        private async Task ShowSchedule()
        {
            var toasts = VM.ToastService.GetAllScheduledToasts();
            var schedule = string.Join(Environment.NewLine, toasts.Select(x => $"'{x.Group}.{x.Tag}' > {x.DeliveryTime}"));
            var dialog = new MessageDialog(schedule, "Schedule");
            await dialog.ShowAsync();
        }
#endif

        protected override async void OnViewModelChanged()
        {
            await VM?.Load();
        }
    }
}
