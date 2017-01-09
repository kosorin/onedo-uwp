using System;
using OneDo.Common.Mvvm;

namespace OneDo.Services.ModalService
{
    public class SubModalService : ExtendedViewModel, IModalService
    {
        private IModal current;
        public IModal Current
        {
            get { return current; }
            private set
            {
                if (Set(ref current, value))
                {
                    RaisePropertyChanged(nameof(CanClose));
                    CurrentChanged?.Invoke(this, EventArgs.Empty);
                    CanCloseChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler CurrentChanged;

        public bool CanClose => Current != null;

        public event EventHandler CanCloseChanged;

        public IExtendedCommand CloseCommand { get; }


        public SubModalService()
        {
            CloseCommand = new RelayCommand(() => TryClose());
        }


        public bool TryClose()
        {
            if (CanClose)
            {
                Close();
                return true;
            }
            return false;
        }

        public void Close()
        {
            Current = null;
        }

        public void Show(IModal modal)
        {
            Current = modal;
        }
    }
}