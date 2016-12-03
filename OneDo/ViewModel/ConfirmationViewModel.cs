using OneDo.Mvvm;
using System;

namespace OneDo.ViewModel
{
    public class ConfirmationViewModel : ModalViewModel
    {
        public event EventHandler ConfirmationRequested;

        public IExtendedCommand ConfirmCommand { get; }

        private string text;
        public string Text
        {
            get { return text; }
            set { Set(ref text, value); }
        }

        private string buttonText;
        public string ButtonText
        {
            get { return buttonText; }
            set { Set(ref buttonText, value); }
        }

        public ConfirmationViewModel()
        {
            ConfirmCommand = new RelayCommand(Confirm);
        }

        public void Confirm()
        {
            ConfirmationRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
