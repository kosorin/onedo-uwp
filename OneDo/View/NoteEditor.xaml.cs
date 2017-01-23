using OneDo.Application.Queries.Notes;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using OneDo.ViewModel.Parameters;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace OneDo.View
{
    public sealed partial class NoteEditor : ModalView, IXBind<NoteEditorViewModel>
    {
        public NoteEditorViewModel VM => ViewModel as NoteEditorViewModel;

        public NoteEditor(NoteEditorParameters parameters) : base(parameters)
        {
            InitializeComponent();
        }

        protected override async Task OnFirstLoad()
        {
            var parameters = (NoteEditorParameters)Parameters;
            await VM.Load(parameters.EntityId);
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
            HideDatePicker();
        }

        private void OnDateButtonTapped(object sender, TappedRoutedEventArgs e)
        {
            ShowDatePicker();
        }

        private void HideDatePicker()
        {
            //VM.SubModalService.TryClose();
        }

        private void ShowDatePicker()
        {
            //VM.SubModalService.Show(VM.DatePicker);
        }


        private void OnReminderChanged(TimePickerViewModel sender, TimePickerEventArgs args)
        {
            HideTimePicker();
        }

        private void ReminderButton_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ShowTimePicker();
        }

        private void HideTimePicker()
        {
            //VM.SubModalService.TryClose();
        }

        private void ShowTimePicker()
        {
            //VM.SubModalService.Show(VM.ReminderPicker);
        }
    }
}
