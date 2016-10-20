using System;
using OneDo.ViewModel;
using OneDo.ViewModel.Args;
using OneDo.ViewModel.Modals;
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
            }
        }

        protected override void OnViewModelChanged()
        {
            if (VM != null)
            {
                VM.DatePicker.DateChanged += OnDateChanged;
            }
        }

        private void OnDateChanged(DatePickerViewModel sender, DatePickerEventArgs args)
        {
            DateButton.Flyout.Hide();
        }
    }
}
