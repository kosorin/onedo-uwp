using GalaSoft.MvvmLight;
using OneDo.ViewModel;
using OneDo.View;
using System;
using System.Linq;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OneDo.Common.Data;
using System.Collections.Specialized;
using Windows.System;
using OneDo.Common.UI;
using OneDo.ViewModel.Commands;

namespace OneDo.Services.ModalService
{
    public class SubModalService : ExtendedViewModel, IModalService
    {
        private ModalViewModel current;
        public ModalViewModel Current
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
            CloseCommand = new RelayCommand(Close, () => CanClose);
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

        public void Show(ModalViewModel modal)
        {
            Current = modal;
        }
    }
}