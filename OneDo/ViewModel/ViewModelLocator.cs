using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;

namespace OneDo.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            // Služby
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, FileDataService>();
            }
            SimpleIoc.Default.Register<INavigationService, FrameNavigationService>();

            // Stránky
            SimpleIoc.Default.Register<StartViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<TodoViewModel>();
        }

        public StartViewModel StartPage => SimpleIoc.Default.GetInstance<StartViewModel>();

        public MainViewModel MainPage => SimpleIoc.Default.GetInstance<MainViewModel>();

        public AboutViewModel AboutPage => SimpleIoc.Default.GetInstance<AboutViewModel>();

        public SettingsViewModel SettingsPage => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public TodoViewModel TodoPage => SimpleIoc.Default.GetInstance<TodoViewModel>();
    }
}
