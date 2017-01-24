using OneDo.Application.Queries.Notes;
using OneDo.Common.Args;
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

        public override ModalContainer SubContainer => ModalContainer;

        public NoteEditor(NoteEditorParameters parameters) : base(parameters)
        {
            InitializeComponent();
        }

        protected override async Task OnFirstLoad()
        {
            var parameters = (NoteEditorParameters)Parameters;
            await VM.Load(parameters.EntityId);
        }


        private void HideDatePicker()
        {
            SubContainer.TryClose();
        }

        private void HideTimePicker()
        {
        }

        private void ShowDatePicker()
        {
            var picker = new DatePicker(VM.Date ?? DateTime.Today);
            picker.DateChanged += DatePicker_DateChanged;
            SubContainer.Show(picker);
        }

        private void ShowTimePicker()
        {
        }


        private void DatePicker_DateChanged(DatePicker sender, DateChangedEventArgs args)
        {
            VM.Date = args.Date;
            HideDatePicker();
        }

        private void OnReminderChanged(TimePickerViewModel sender, TimePickerEventArgs args)
        {
            HideTimePicker();
        }

        private void DateButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowDatePicker();
        }

        private void ReminderButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowTimePicker();
        }
    }
}
