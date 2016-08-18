using OneDo.ViewModel;
using OneDo.View;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.Services.ModalService
{
    public interface IModalService
    {
        bool TryPop();

        void Pop();

        void Push(ModalViewModel modal);
    }
}
