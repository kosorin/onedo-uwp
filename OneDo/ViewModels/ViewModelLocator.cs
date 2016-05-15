using GalaSoft.MvvmLight.Ioc;
using OneDo.Model.DataAccess;
using OneDo.Services.NavigationService;

namespace OneDo.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            // Služby
            SimpleIoc.Default.Register<IDataProvider, DataProvider>();
            SimpleIoc.Default.Register<INavigationService, FrameNavigationService>();

            // Stránky
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<TodoViewModel>();
        }

        public MainViewModel MainPage => SimpleIoc.Default.GetInstance<MainViewModel>();

        public AboutViewModel AboutPage => SimpleIoc.Default.GetInstance<AboutViewModel>();

        public SettingsViewModel SettingsPage => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public TodoViewModel TodoPage => SimpleIoc.Default.GetInstance<TodoViewModel>();
    }
}
