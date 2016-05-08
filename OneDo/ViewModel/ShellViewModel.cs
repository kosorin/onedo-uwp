using GalaSoft.MvvmLight;
using OneDo.Services.NavigationService;
using Windows.UI.Xaml.Controls;

namespace OneDo.ViewModel
{
    public class ShellViewModel : ViewModelBase
    {
        public Frame MainFrame => NavigationService.Frame;

        public INavigationService NavigationService { get; }

        public ShellViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }
}
