using GalaSoft.MvvmLight;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using OneDo.View.Pages;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace OneDo.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        public Frame MainFrame => NavigationService.Frame;

        public INavigationService NavigationService { get; }

        public IDataService DataService { get; }

        public ShellViewModel(INavigationService navigationService, IDataService dataService)
        {
            NavigationService = navigationService;
            DataService = dataService;

            NavigationService.Navigate(typeof(MainPage));
        }
    }
}
