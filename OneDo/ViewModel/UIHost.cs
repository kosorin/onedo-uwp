using OneDo.Services.InfoService;
using OneDo.Services.ProgressService;
using System;

namespace OneDo.ViewModel
{
    public class UIHost
    {
        public IProgressService ProgressService { get; }

        public IInfoService InfoService { get; }

        public UIHost(IProgressService progressService, IInfoService infoService)
        {
            ProgressService = progressService;
            InfoService = infoService;
        }
    }
}
