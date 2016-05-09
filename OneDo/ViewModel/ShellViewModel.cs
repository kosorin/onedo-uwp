using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using OneDo.View.Pages;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace OneDo.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        public Frame MainFrame => NavigationService?.Frame;

        public ICommand NavigateMainPageCommand { get; }

        public INavigationService NavigationService { get; }

        public IDataService DataService { get; }

        public ShellViewModel()
        //public ShellViewModel(INavigationService navigationService, IDataService dataService)
        {
            //NavigationService = navigationService;
            //DataService = dataService;

            //NavigateMainPageCommand = new RelayCommand(() => NavigationService.Navigate<MainPage>());
            //NavigationService.Navigate(typeof(MainPage));
        }
    }
}
