using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using OneDo.Model.Data;
using OneDo.Services.Context;
using OneDo.Services.NavigationService;
using SimpleInjector;

namespace OneDo.ViewModels
{
    public class ViewModelLocator
    {
        public static Container Container { get; }

        static ViewModelLocator()
        {
            var container = new Container();

            // Služby
            if (ViewModelBase.IsInDesignModeStatic)
            {
                container.Register<IDataProvider, DesignDataProvider>(Lifestyle.Singleton);
            }
            else
            {
                container.Register<IDataProvider, DataProvider>(Lifestyle.Singleton);
            }
            container.Register<INavigationService, FrameNavigationService>(Lifestyle.Singleton);
            container.Register<IContext, Context>(Lifestyle.Singleton);

            // Stránky
            container.Register<MainViewModel>(Lifestyle.Singleton);
            container.Register<AboutViewModel>(Lifestyle.Singleton);
            container.Register<SettingsViewModel>(Lifestyle.Singleton);
            container.Register<TodoViewModel>(Lifestyle.Transient);

            Container = container;
        }

        public MainViewModel MainPage => Container.GetInstance<MainViewModel>();

        public AboutViewModel AboutPage => Container.GetInstance<AboutViewModel>();

        public SettingsViewModel SettingsPage => Container.GetInstance<SettingsViewModel>();

        public TodoViewModel TodoPage => Container.GetInstance<TodoViewModel>();
    }
}
