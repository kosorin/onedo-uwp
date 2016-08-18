using GalaSoft.MvvmLight;
using OneDo.Common.Logging;
using OneDo.Model.Data;
using OneDo.Services.ModalService;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System;

namespace OneDo.ViewModel
{
    public abstract class PageViewModel : ExtendedViewModel
    {
        public IModalService ModalService { get; }

        public ISettingsProvider SettingsProvider { get; }

        protected PageViewModel(IModalService modalService, ISettingsProvider settingsProvider)
        {
            ModalService = modalService;
            SettingsProvider = settingsProvider;
        }
    }
}
