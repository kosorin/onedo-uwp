using GalaSoft.MvvmLight;
using OneDo.Common.Logging;
using OneDo.Model.Data;
using OneDo.Services.NavigationService;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System;

namespace OneDo.ViewModels
{
    public abstract class PageViewModel : ExtendedViewModel, INavigable
    {
        public INavigationService NavigationService { get; }

        public ISettingsProvider SettingsProvider { get; }

        protected PageViewModel(INavigationService navigationService, ISettingsProvider settingsProvider)
        {
            NavigationService = navigationService;
            SettingsProvider = settingsProvider;
        }


        public virtual void OnNavigatingFrom(NavigatingEventArgs args)
        {

        }

        public virtual Task OnNavigatedFromAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void OnNavigatedTo(object parameter, NavigationMode mode)
        {

        }

        public virtual void OnBackButton(BackButtonEventArgs args)
        {

        }
    }
}
