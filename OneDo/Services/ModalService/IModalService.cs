using OneDo.Common.Mvvm;
using System;

namespace OneDo.Services.ModalService
{
    [Obsolete("Otevírání Modalů se řeší přes Messanger")]
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
