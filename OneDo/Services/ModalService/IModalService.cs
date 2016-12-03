using System;
using OneDo.Mvvm;

namespace OneDo.Services.ModalService
{
    public interface IModalService
    {
        IModal Current { get; }

        event EventHandler CurrentChanged;

        bool CanClose { get; }

        event EventHandler CanCloseChanged;

        IExtendedCommand CloseCommand { get; }

        bool TryClose();

        void Close();

        void Show(IModal modal);
    }
}
