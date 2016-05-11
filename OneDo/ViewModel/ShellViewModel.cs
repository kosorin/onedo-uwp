using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using OneDo.View.Pages;
using OneDo.ViewModel.Commands;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using System;

namespace OneDo.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        public Frame MainFrame { get; }

        private bool isOpen;
        public bool IsOpen
        {
            get { return isOpen; }
            set { Set(ref isOpen, value); }
        }

        public ICommand NavigateToMainPageCommand { get; }

        public ICommand NavigateToAboutPageCommand { get; }

        public INavigationService NavigationService { get; }

        public IDataService DataService { get; }

        public ShellViewModel(INavigationService navigationService, IDataService dataService)
        {
            NavigationService = navigationService;
            DataService = dataService;

            MainFrame = NavigationService.Frame;

            NavigateToMainPageCommand = new NavigationCommand<MainPage>(NavigationService);
            NavigateToAboutPageCommand = new NavigationCommand<AboutPage>(NavigationService);
        }
    }
}
