using GalaSoft.MvvmLight;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Windows.ApplicationModel;
using OneDo.View.Pages;

namespace OneDo.ViewModel.Pages
{
    public class StartPageViewModel : BasePageViewModel
    {
        public IDataService DataService { get; }

        public StartPageViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService)
        {
            DataService = dataService;
        }
    }
}