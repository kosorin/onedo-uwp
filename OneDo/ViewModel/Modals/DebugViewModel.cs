using OneDo.Model.Data;
using OneDo.Services.ModalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Modals
{
    public class DebugViewModel : ModalViewModel
    {
        public DebugViewModel(IModalService modalService, ISettingsProvider settingsProvider) : base(modalService, settingsProvider)
        {

        }
    }
}
