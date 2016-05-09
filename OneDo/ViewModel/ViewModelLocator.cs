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
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Služby
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }
            SimpleIoc.Default.Register<INavigationService, ShellNavigationService>();

            // Shell
            SimpleIoc.Default.Register<ShellViewModel>();

            // Stránky
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<AboutPageViewModel>();
        }

        public ShellViewModel Shell => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public MainPageViewModel MainPage => ServiceLocator.Current.GetInstance<MainPageViewModel>();

        public AboutPageViewModel AboutPage => ServiceLocator.Current.GetInstance<AboutPageViewModel>();
    }
}
