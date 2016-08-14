using GalaSoft.MvvmLight;
using OneDo.Model.Data;
using OneDo.Services.NavigationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public abstract class ModalViewModel : ExtendedViewModel
    {
        public INavigationService NavigationService { get; }

        public ISettingsProvider SettingsProvider { get; }

        protected ModalViewModel(INavigationService navigationService, ISettingsProvider settingsProvider)
        {
            NavigationService = navigationService;
            SettingsProvider = settingsProvider;
        }
    }
}
