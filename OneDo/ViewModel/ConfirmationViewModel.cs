using OneDo.Common.UI;
using OneDo.Model.Data;
using OneDo.Services.ModalService;
using OneDo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace OneDo.ViewModel
{
    public class ConfirmationViewModel : ModalViewModel
    {
        public event EventHandler ConfirmationRequested;

        public IExtendedCommand ConfirmCommand { get; }

        public ConfirmationViewModel()
        {
            ConfirmCommand = new RelayCommand(OnConfirm);
        }

        private void OnConfirm()
        {
            ConfirmationRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
