using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using OneDo.ViewModel.Pages;
using Windows.ApplicationModel;

namespace OneDo.ViewModel
{
    public class ViewModelLocator
    {
        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public StartPageViewModel StartPage => ServiceLocator.Current.GetInstance<StartPageViewModel>();

        public MainPageViewModel MainPage => ServiceLocator.Current.GetInstance<MainPageViewModel>();

        public AboutPageViewModel AboutPage => ServiceLocator.Current.GetInstance<AboutPageViewModel>();
    }
}
