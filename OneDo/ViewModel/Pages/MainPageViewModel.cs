using GalaSoft.MvvmLight;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModel.Pages
{
    public class MainPageViewModel : PageViewModelBase
    {
        public string Text { get; set; } = "Karel";

        public IDataService DataService { get; }

        public MainPageViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService)
        {
            DataService = dataService;
            DataService.LoadAsync();
        }
    }
}
