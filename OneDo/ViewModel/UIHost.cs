using OneDo.Services.InfoService;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class UIHost
    {
        public IModalService ModalService { get; }

        public IProgressService ProgressService { get; }

        public IInfoService InfoService { get; }

        public UIHost(IModalService modalService, IProgressService progressService, IInfoService infoService)
        {
            ModalService = modalService;
            ProgressService = progressService;
            InfoService = infoService;
        }
    }
}
