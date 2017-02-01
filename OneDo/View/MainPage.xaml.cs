﻿using System;
using OneDo.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;
using OneDo.Services.InfoService;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Linq;
using OneDo.Services.ToastService;
using OneDo.ViewModel.Parameters;
using OneDo.Application.Queries.Folders;
using OneDo.Common;
using OneDo.Application.Queries.Notes;

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
            InitializeNavigation();

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
            ModalContainer.AddFadeInAnimation<NoteEditor>("Offset.Y", noteEditorFadeInAnimation);

            var noteEditorFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            noteEditorFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            noteEditorFadeOutAnimation.InsertKeyFrame(0, 0);
            noteEditorFadeOutAnimation.InsertExpressionKeyFrame(1, "Height", ModalContainer.DefaultEasing);
            ModalContainer.AddFadeOutAnimation<NoteEditor>("Offset.Y", noteEditorFadeOutAnimation);

            var settingsFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            settingsFadeInAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            settingsFadeInAnimation.InsertExpressionKeyFrame(0, "Width");
            settingsFadeInAnimation.InsertKeyFrame(1, 0, ModalContainer.DefaultEasing);
            ModalContainer.AddFadeInAnimation<SettingsView>("Offset.X", settingsFadeInAnimation);

            var settingsFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            settingsFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            settingsFadeOutAnimation.InsertKeyFrame(0, 0);
            settingsFadeOutAnimation.InsertExpressionKeyFrame(1, "Width", ModalContainer.DefaultEasing);
            ModalContainer.AddFadeOutAnimation<SettingsView>("Offset.X", settingsFadeOutAnimation);
        }

        private void InitializeInfoBar()
        {
            Messenger.Default.Register<InfoMessage>(InfoBar, InfoBar.Show);
        }

        private void InitializeNavigation()
        {
            Messenger.Default.Register<SettingsParameters>(this, Handle);
            Messenger.Default.Register<DebugParameters>(this, Handle);
            Messenger.Default.Register<FolderEditorParameters>(this, Handle);
            Messenger.Default.Register<NoteEditorParameters>(this, Handle);
        }


        private void Handle(SettingsParameters args)
        {
            ModalContainer.Show(new SettingsView());
        }

        private void Handle(DebugParameters args)
        {
            ModalContainer.Show(new DebugView());
        }

        private void Handle(FolderEditorParameters args)
        {
            ModalContainer.Show(new FolderEditor(args.EntityId));
        }

        private void Handle(NoteEditorParameters args)
        {
            ModalContainer.Show(new NoteEditor(args.EntityId));
        }


#if DEBUG
        private void InitializeDebug()
        {
            InsertMenuSeparator();
            InsertMenuButtonAsync("Reset", VM.ResetData);
            InsertMenuButton("Switch theme", SwitchRequestedTheme);
            InsertMenuButtonAsync("Show schedule", ShowSchedule);
            InsertMenuButton("Debug", () => Messenger.Default.Send(new DebugParameters()));
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
                switch (Windows.UI.Xaml.Application.Current.RequestedTheme)
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
            var toastService = new ToastService();
            var toasts = toastService.GetScheduledToasts();
            var schedule = string.Join(Environment.NewLine, toasts.Select(x => $"'{x.Group}.{x.Tag}' > {x.DeliveryTime}"));

            var clearCommand = new UICommand("Clear", x => toastService.ClearSchedule());
            var closeCommand = new UICommand("Cancel");

            var dialog = new MessageDialog(schedule, "Schedule");
            dialog.Commands.Add(clearCommand);
            dialog.Commands.Add(closeCommand);
            dialog.CancelCommandIndex = (uint)dialog.Commands.Count - 1;
            dialog.DefaultCommandIndex = (uint)dialog.Commands.Count - 1;
            var command = await dialog.ShowAsync();
        }
#endif
    }
}
