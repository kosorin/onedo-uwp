using System;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using System.Numerics;
using Windows.UI.Composition;
using OneDo.Services.ModalService;

namespace OneDo.View
{
    public sealed partial class NoteEditor : ModalBase, IXBind<NoteEditorViewModel>
    {
        public NoteEditorViewModel VM => ViewModel as NoteEditorViewModel;


        public NoteEditor()
        {
            InitializeComponent();
            InitializeModalAnimations();
        }

        private void InitializeModalAnimations()
        {
            var confirmationFadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            confirmationFadeInAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            confirmationFadeInAnimation.InsertKeyFrame(0, 50);
            confirmationFadeInAnimation.InsertKeyFrame(1, 0, ModalContainer.DefaultEasing);
            ModalContainer.AddFadeInAnimation<ConfirmationViewModel>("Offset.Y", confirmationFadeInAnimation);

            var confirmationFadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            confirmationFadeOutAnimation.Duration = TimeSpan.FromMilliseconds(ModalContainer.DefaultDuration);
            confirmationFadeOutAnimation.InsertKeyFrame(0, 0);
            confirmationFadeOutAnimation.InsertKeyFrame(1, 50, ModalContainer.DefaultEasing);
            ModalContainer.AddFadeOutAnimation<ConfirmationViewModel>("Offset.Y", confirmationFadeOutAnimation);
        }


        protected override void OnViewModelChanging()
        {
            if (VM != null)
            {
                VM.DatePicker.DateChanged -= OnDateChanged;
                VM.ReminderPicker.TimeChanged -= OnReminderChanged;
            }
        }

        protected override void OnViewModelChanged()
        {
            if (VM != null)
            {
                VM.DatePicker.DateChanged += OnDateChanged;
                VM.ReminderPicker.TimeChanged += OnReminderChanged;
            }
        }


        private void OnDateChanged(DatePickerViewModel sender, DatePickerEventArgs args)
        {
            CloseDatePicker();
        }

        private void DateButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowDatePicker();
        }

        private void CloseDatePicker()
        {
            VM.SubModalService.TryClose();
        }

        private void ShowDatePicker()
        {
            if (VM.DatePicker.Date == null)
            {
                VM.DatePicker.DateChanged -= OnDateChanged;
                VM.DatePicker.Date = DateTime.Today;
                VM.DatePicker.DateChanged += OnDateChanged;
            }
            VM.SubModalService.Show(VM.DatePicker);
        }


        private void OnReminderChanged(TimePickerViewModel sender, TimePickerEventArgs args)
        {
            //ReminderPickerFlyout.Hide();
        }

        private void ReminderButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //ReminderPickerFlyout.ShowAt(ReminderPickerGrid);
        }
    }
}
