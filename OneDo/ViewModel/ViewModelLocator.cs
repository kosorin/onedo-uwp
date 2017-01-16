using Autofac;
using OneDo.Application;
using OneDo.Services.BackgroundTaskService;
using OneDo.Services.InfoService;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using OneDo.Services.StringProvider;
using OneDo.Services.ToastService;
using Windows.ApplicationModel;

namespace OneDo.ViewModel
{
#warning Pøedìlat ViewModelLocator ze statického ServiceLocatoru na nìco rozumného
    public class ViewModelLocator
    {
        public static IContainer Container { get; }

        static ViewModelLocator()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                var builder = new ContainerBuilder();

                builder.RegisterType<BackgroundTaskService>().As<IBackgroundTaskService>().SingleInstance();
                builder.RegisterType<ToastService>().As<IToastService>().SingleInstance();
                builder.RegisterType<ModalService>().As<IModalService>().SingleInstance();
                builder.RegisterType<ProgressService>().As<IProgressService>().SingleInstance();
                builder.RegisterType<InfoService>().As<IInfoService>().SingleInstance();
                builder.RegisterType<StringProvider>().As<IStringProvider>().SingleInstance();

                builder.RegisterType<UIHost>().SingleInstance();
                builder.RegisterType<MainViewModel>().SingleInstance();

                builder.RegisterType<Api>().AsSelf().SingleInstance();

                Container = builder.Build();
            }
        }

        public MainViewModel Main => Container?.Resolve<MainViewModel>();
    }
}
