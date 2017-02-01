using GalaSoft.MvvmLight.Messaging;
using OneDo.Services.InfoService;
using OneDo.Services.ProgressService;
using System;

namespace OneDo.Core
{
    public class UIHost
    {
        public IProgressService ProgressService { get; }

        public IInfoService InfoService { get; }

        public IMessenger Messenger { get; }

        public UIHost(IProgressService progressService, IInfoService infoService)
        {
            ProgressService = progressService;
            InfoService = infoService;
            Messenger = new Messenger();
        }
    }
}
