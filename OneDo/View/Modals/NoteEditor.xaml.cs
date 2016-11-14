using System;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using OneDo.ViewModel.Modals;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.View.Modals
{
    public sealed partial class NoteEditor : ModalBase, IXBind<NoteEditorViewModel>
    {
        public NoteEditorViewModel VM => ViewModel as NoteEditorViewModel;

        public NoteEditor()
        {
            InitializeComponent();
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
            //DatePickerFlyout.Hide();
            DatePickerBorder.Visibility = Visibility.Collapsed;
        }

        private void OnReminderChanged(TimePickerViewModel sender, TimePickerEventArgs args)
        {
            //ReminderPickerFlyout.Hide();
        }

        private void DateButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //DatePickerFlyout.ShowAt(DatePickerGrid);
            if (VM.DatePicker.Date == null)
            {
                VM.DatePicker.Date = DateTime.Today;
            }
            DatePickerBorder.Visibility = Visibility.Visible;
        }

        private void ReminderButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            //ReminderPickerFlyout.ShowAt(ReminderPickerGrid);
        }
    }
}
