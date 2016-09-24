using OneDo.ViewModel;
using OneDo.View;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace OneDo.Services.ModalService
{
    public interface IModalService
    {
        ModalViewModel Current { get; }

        bool CanClose { get; }

        event EventHandler CurrentChanged;

        ICommand CloseCommand { get; }

        bool TryClose();

        void Close();

        void Show(ModalViewModel modal);
    }
}
