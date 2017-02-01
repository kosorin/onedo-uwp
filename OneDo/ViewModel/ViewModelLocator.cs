using Autofac;
using OneDo.Application;
using OneDo.Core;
using OneDo.Services.BackgroundTaskService;
using OneDo.Services.InfoService;
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

                builder.RegisterType<Api>().AsSelf().SingleInstance();
                builder.RegisterType<UIHost>().AsSelf().SingleInstance();
                builder.RegisterType<BackgroundTaskService>().As<IBackgroundTaskService>().SingleInstance();
                builder.RegisterType<ToastService>().As<IToastService>().SingleInstance();
                builder.RegisterType<ProgressService>().As<IProgressService>().SingleInstance();
                builder.RegisterType<InfoService>().As<IInfoService>().SingleInstance();
                builder.RegisterType<StringProvider>().As<IStringProvider>().SingleInstance();

                builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
                builder.RegisterType<FolderListViewModel>().AsSelf().SingleInstance();
                builder.RegisterType<NoteListViewModel>().AsSelf().SingleInstance();

                builder.RegisterType<SettingsViewModel>().AsSelf().InstancePerDependency();
                builder.RegisterType<DebugViewModel>().AsSelf().InstancePerDependency();
                builder.RegisterType<FolderEditorViewModel>().AsSelf().InstancePerDependency();
                builder.RegisterType<NoteEditorViewModel>().AsSelf().InstancePerDependency();

                Container = builder.Build();
            }
        }

        public MainViewModel Main => Container?.Resolve<MainViewModel>();

        public SettingsViewModel Settings => Container?.Resolve<SettingsViewModel>();

        public DebugViewModel Debug => Container?.Resolve<DebugViewModel>();

        public FolderListViewModel FolderList => Container?.Resolve<FolderListViewModel>();

        public NoteListViewModel NoteList => Container?.Resolve<NoteListViewModel>();

        public FolderEditorViewModel FolderEditor => Container?.Resolve<FolderEditorViewModel>();

        public NoteEditorViewModel NoteEditor => Container?.Resolve<NoteEditorViewModel>();
    }
}
