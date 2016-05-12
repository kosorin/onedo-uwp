using GalaSoft.MvvmLight;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace OneDo.ViewModel.Pages
{
    public class MainPageViewModel : BasePageViewModel
    {
        public ICommand TestCommand { get; }

        public IDataService DataService { get; }

        public MainPageViewModel(ShellViewModel shell, INavigationService navigationService, IDataService dataService)
            : base(shell, navigationService)
        {
            DataService = dataService;
        }
    }
}
