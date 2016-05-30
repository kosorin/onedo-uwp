using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using OneDo.Model.Data;
using OneDo.Services.Context;
using OneDo.Services.NavigationService;
using OneDo.ViewModels.Editors;
using OneDo.ViewModels.Pages;
using OneDo.ViewModels.Settings;
using SimpleInjector;

namespace OneDo.ViewModels
{
    public class ViewModelLocator
    {
        public static Container Container { get; }

        static ViewModelLocator()
        {
            var container = new Container();

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

            container.Register<MainViewModel>(Lifestyle.Singleton);
            container.Register<AboutViewModel>(Lifestyle.Singleton);
            container.Register<SettingsViewModel>(Lifestyle.Singleton);
            container.Register<TodoViewModel>(Lifestyle.Transient);

            Container = container;
        }

        public MainViewModel MainPage => Container.GetInstance<MainViewModel>();

        public AboutViewModel About => Container.GetInstance<AboutViewModel>();

        public SettingsViewModel Settings => Container.GetInstance<SettingsViewModel>();

        public TodoViewModel TodoEditor => Container.GetInstance<TodoViewModel>();
    }
}
