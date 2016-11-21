using OneDo.ViewModel;
using OneDo.View;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using OneDo.Common.UI;

namespace OneDo.Services.ModalService
{
    public interface IModalService
    {
        ModalViewModel Current { get; }

        event EventHandler CurrentChanged;

        bool CanClose { get; }

        event EventHandler CanCloseChanged;

        IExtendedCommand CloseCommand { get; }

        bool TryClose();

        void Close();

        void Show(ModalViewModel modal);
    }
}
