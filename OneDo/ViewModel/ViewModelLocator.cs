using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;

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

            // Stránky
            SimpleIoc.Default.Register<StartViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<TodoViewModel>();
        }

        public StartViewModel StartPage => ServiceLocator.Current.GetInstance<StartViewModel>();

        public MainViewModel MainPage => ServiceLocator.Current.GetInstance<MainViewModel>();

        public AboutViewModel AboutPage => ServiceLocator.Current.GetInstance<AboutViewModel>();

        public SettingsViewModel SettingsPage => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public TodoViewModel TodoPage => ServiceLocator.Current.GetInstance<TodoViewModel>();
    }
}
