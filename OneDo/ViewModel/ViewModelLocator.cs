using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using OneDo.Model.Data;
using OneDo.Services.NavigationService;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Flyouts;
using OneDo.ViewModel.Pages;
using Windows.UI.Xaml;

namespace OneDo.ViewModel
{
    public class ViewModelLocator
    {
        public static IContainer Container { get; }

        static ViewModelLocator()
        {
            if (ViewModelBase.IsInDesignModeStatic) return;

            var builder = new ContainerBuilder();

            builder.RegisterType<ProgressService>().As<IProgressService>().SingleInstance();
            builder.RegisterType<SettingsProvider>().As<ISettingsProvider>().SingleInstance();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();

            builder.RegisterType<MainViewModel>().SingleInstance();

            Container = builder.Build();
        }

        public IProgressService ProgressService => Container?.Resolve<IProgressService>();

        public INavigationService NavigationService => Container?.Resolve<INavigationService>();

        public MainViewModel MainPage => Container?.Resolve<MainViewModel>();
    }
}
