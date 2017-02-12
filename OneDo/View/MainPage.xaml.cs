using System;
using OneDo.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;
using OneDo.Services.InfoService;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Linq;
using OneDo.Application.Queries.Folders;
using OneDo.Common;
using OneDo.Application.Queries.Notes;
using OneDo.Core.Messages;
using Windows.UI.Notifications;

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
            InitializeModal();

#if DEBUG
            InitializeLog();
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

        private void InitializeModal()
        {
            Messenger.Default.Register<CloseModalMessage>(this, HandleModal);
            Messenger.Default.Register<ShowSettingsMessage>(this, HandleModal);
            Messenger.Default.Register<ShowLogMessage>(this, HandleModal);
            Messenger.Default.Register<ShowFolderEditorMessage>(this, HandleModal);
            Messenger.Default.Register<ShowNoteEditorMessage>(this, HandleModal);
        }


        private void HandleModal(CloseModalMessage message)
        {
            ModalContainer.TryClose();
        }

        private void HandleModal(ShowSettingsMessage message)
        {
            ModalContainer.Show(new SettingsView());
        }

        private void HandleModal(ShowLogMessage message)
        {
            ModalContainer.Show(new LogView());
        }

        private void HandleModal(ShowFolderEditorMessage message)
        {
            ModalContainer.Show(new FolderEditor(message.Id));
        }

        private void HandleModal(ShowNoteEditorMessage message)
        {
            ModalContainer.Show(new NoteEditor(message.Id));
        }


#if DEBUG
        private void InitializeLog()
        {
            InsertMenuSeparator();
            InsertMenuButtonAsync("Reset", VM.ResetData);
            InsertMenuButton("Switch theme", SwitchRequestedTheme);
            InsertMenuButtonAsync("Schedule", ShowSchedule);
            InsertMenuButton("Log", () => Messenger.Default.Send(new ShowLogMessage()));
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
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var toasts = notifier.GetScheduledToastNotifications();
            var schedule = string.Join(Environment.NewLine, toasts.Select(x => $"'{x.Group}.{x.Tag}' > {x.DeliveryTime}"));

            var clearCommand = new UICommand("Clear", x => ClearSchedule());
            var closeCommand = new UICommand("Cancel");

            var dialog = new MessageDialog(schedule, "Schedule");
            dialog.Commands.Add(clearCommand);
            dialog.Commands.Add(closeCommand);
            dialog.CancelCommandIndex = (uint)dialog.Commands.Count - 1;
            dialog.DefaultCommandIndex = (uint)dialog.Commands.Count - 1;
            var command = await dialog.ShowAsync();
        }

        private void ClearSchedule()
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var toast in notifier.GetScheduledToastNotifications())
            {
                notifier.RemoveFromSchedule(toast);
            }
        }
#endif
    }
}
