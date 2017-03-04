using Autofac;
using OneDo.Application;
using OneDo.Core;
using OneDo.Services.BackgroundTaskService;
using OneDo.Services.InfoService;
using OneDo.Services.ProgressService;
using OneDo.Services.StringProvider;
using OneDo.ViewModel.Folder;
using OneDo.ViewModel.Note;
using OneDo.ViewModel.Settings;
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

                builder.RegisterType<Api>().As<IApi>().SingleInstance();
                builder.RegisterType<UIHost>().AsSelf().SingleInstance();
                builder.RegisterType<BackgroundTaskService>().As<IBackgroundTaskService>().SingleInstance();
                builder.RegisterType<ProgressService>().As<IProgressService>().SingleInstance();
                builder.RegisterType<InfoService>().As<IInfoService>().SingleInstance();
                builder.RegisterType<StringProvider>().As<IStringProvider>().SingleInstance();

                builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
                builder.RegisterType<LogViewModel>().AsSelf().InstancePerDependency();

                builder.RegisterType<SettingsViewModel>().AsSelf().InstancePerDependency();

                builder.RegisterType<FolderListViewModel>().AsSelf().SingleInstance();
                builder.RegisterType<FolderEditorViewModel>().AsSelf().InstancePerDependency();

                builder.RegisterType<NoteListViewModel>().AsSelf().SingleInstance();
                builder.RegisterType<NoteEditorViewModel>().AsSelf().InstancePerDependency();

                Container = builder.Build();
            }
        }

        public MainViewModel Main => Container?.Resolve<MainViewModel>();

        public LogViewModel Log => Container?.Resolve<LogViewModel>();

        public SettingsViewModel Settings => Container?.Resolve<SettingsViewModel>();

        public FolderListViewModel FolderList => Container?.Resolve<FolderListViewModel>();

        public FolderEditorViewModel FolderEditor => Container?.Resolve<FolderEditorViewModel>();

        public NoteListViewModel NoteList => Container?.Resolve<NoteListViewModel>();

        public NoteEditorViewModel NoteEditor => Container?.Resolve<NoteEditorViewModel>();
    }
}
