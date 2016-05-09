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
        public Frame MainFrame => NavigationService?.Frame;

        public ICommand NavigateToMainPageCommand { get; }

        public ICommand NavigateToAboutPageCommand { get; }

        public ICommand LoadedCommand { get; }

        public INavigationService NavigationService { get; }

        public IDataService DataService { get; }

        public ShellViewModel(INavigationService navigationService, IDataService dataService)
        {
            NavigationService = navigationService;
            DataService = dataService;

            NavigateToMainPageCommand = new NavigationCommand<MainPage>(NavigationService);
            NavigateToAboutPageCommand = new NavigationCommand<AboutPage>(NavigationService);
            LoadedCommand = new RelayCommand(Loaded);
        }

        private async void Loaded()
        {
            await DataService.LoadAsync();
            await new MessageDialog("Data loaded.").ShowAsync();
        }
    }
}
