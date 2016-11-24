using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using OneDo.Model.Business;
using OneDo.Model.Data;
using OneDo.Services.InfoService;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using OneDo.Services.StringProvider;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace OneDo.ViewModel
{
    public class ViewModelLocator
    {
        public static IContainer Container { get; }

        static ViewModelLocator()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                var builder = new ContainerBuilder();

                builder.RegisterType<DataService>().SingleInstance();

                builder.RegisterType<StringProvider>().As<IStringProvider>().SingleInstance();

                builder.RegisterType<UIHost>().SingleInstance();
                builder.RegisterType<ModalService>().As<IModalService>().SingleInstance();
                builder.RegisterType<ProgressService>().As<IProgressService>().SingleInstance();
                builder.RegisterType<InfoService>().As<IInfoService>().SingleInstance();

                builder.RegisterType<DateTimeBusiness>().SingleInstance();

                builder.RegisterType<MainViewModel>().SingleInstance();

                Container = builder.Build();
            }
        }

        public IProgressService ProgressService => Container?.Resolve<IProgressService>();

        public IModalService ModalService => Container?.Resolve<IModalService>();

        public MainViewModel Main => Container?.Resolve<MainViewModel>();
    }
}
