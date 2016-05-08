using GalaSoft.MvvmLight;
using OneDo.Model;
using OneDo.Model.Recurrences;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModel.Pages
{
    public class MainPageViewModel : ViewModelBase, INavigable
    {
        public string Text { get; set; } = "Karel";

        public IDataService DataService { get; }

        public MainPageViewModel(IDataService dataService)
        {
            DataService = dataService;
            //DataService.LoadAsync();
        }


        public void OnNavigatingFrom(NavigatingEventArgs args)
        {

        }

        public Task OnNavigatedFromAsync()
        {
            return Task.CompletedTask;
        }

        public void OnNavigatedTo(object parameter, NavigationMode mode)
        {

        }

        public void OnBackButton(BackButtonEventArgs args)
        {

        }
    }
}
