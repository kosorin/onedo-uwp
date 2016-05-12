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

        private bool isPaneOpen;
        public bool IsPaneOpen
        {
            get { return isPaneOpen; }
            set { Set(ref isPaneOpen, value); }
        }


        public ICommand TogglePaneCommand { get; }

        public ICommand NavigateToMainPageCommand { get; }

        public ICommand NavigateToAboutPageCommand { get; }

        public INavigationService NavigationService { get; }

        public IDataService DataService { get; }

        public ShellViewModel(INavigationService navigationService, IDataService dataService)
        {
            NavigationService = navigationService;
            DataService = dataService;

            MainFrame = NavigationService.Frame;
            MainFrame.Navigated += (s, e) => IsPaneOpen = false;

            TogglePaneCommand = new RelayCommand(() => IsPaneOpen = !IsPaneOpen);
            NavigateToMainPageCommand = new NavigationCommand<MainPage>(NavigationService);
            NavigateToAboutPageCommand = new NavigationCommand<AboutPage>(NavigationService);
        }
    }
}
