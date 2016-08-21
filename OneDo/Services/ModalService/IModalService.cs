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
        ObservableCollection<ModalViewModel> Items { get; }

        ModalViewModel CurrentItem { get; }

        bool CanPop { get; }

        ICommand PopCommand { get; }

        bool TryPop();

        void Pop();

        void Push(ModalViewModel modal);
    }
}
