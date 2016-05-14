﻿using GalaSoft.MvvmLight;
using OneDo.Services.NavigationService;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModel
{
    public abstract class PageViewModel : ViewModelBase, INavigable
    {
        public INavigationService NavigationService { get; }

        protected PageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
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
