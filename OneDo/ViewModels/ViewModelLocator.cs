using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using OneDo.Model.Data;
using OneDo.Services.Context;
using OneDo.Services.NavigationService;
using OneDo.ViewModels.Flyouts;
using OneDo.ViewModels.Pages;
using Windows.UI.Xaml;

namespace OneDo.ViewModels
{
    public class ViewModelLocator
    {
        public static IContainer Container { get; }

        static ViewModelLocator()
        {
            var builder = new ContainerBuilder();

            if (ViewModelBase.IsInDesignModeStatic)
            {
                builder.RegisterType<DesignDataProvider>().As<IDataProvider>().SingleInstance();
            }
            else
            {
                builder.RegisterType<DataProvider>().As<IDataProvider>().SingleInstance();
            }
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<Context>().As<IContext>().SingleInstance();

            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<AboutViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();

            Container = builder.Build();
        }

        public MainViewModel MainPage => Container.Resolve<MainViewModel>();

        public AboutViewModel About => Container.Resolve<AboutViewModel>();

        public SettingsViewModel Settings => Container.Resolve<SettingsViewModel>();
    }
}
