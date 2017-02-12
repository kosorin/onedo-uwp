using OneDo.Common.Logging;
using OneDo.View;
using OneDo.ViewModel;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.Storage;
using Autofac;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.UI;
using OneDo.Common.Meta;
using Windows.Globalization;
using OneDo.Services.BackgroundTaskService;
using OneDo.Core.BackgroundTasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using OneDo.Application;
using OneDo.Application.Commands.Folders;
using System.Linq;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Models;

namespace OneDo
{
    sealed partial class App : Windows.UI.Xaml.Application
    {
        static App()
        {
            RuntimeHelpers.RunClassConstructor(typeof(ViewModelLocator).TypeHandle);
        }

        public App()
        {
#if DEBUG
            ApplicationLanguages.PrimaryLanguageOverride = "cs-CZ";
#endif

            InitializeComponent();
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            Suspending += async (s, e) =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
            {
                var deferral = e.SuspendingOperation.GetDeferral();
                try
                {
                    await OnSuspendingAsync(e.SuspendingOperation.Deadline);
                }
                finally
                {
                    deferral.Complete();
                }
            };

            Resuming += (s, e) =>
            {
                OnResuming(e);
            };

            UnhandledException += (s, e) =>
            {
                OnUnhandledException(e);
            };

#if DEBUG
            ApplicationView.PreferredLaunchViewSize = new Size(480, 800);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
#endif
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await InitializeLogger();

            Logger.Current.Line("=================================");
            Logger.Current.Info($"Version: {AppInformation.GetVersion()}");
            Logger.Current.Info($"Arguments: \"{args.Arguments}\"");
            Logger.Current.Info($"Launch reason: {args.Kind}");
            Logger.Current.Info($"Tile ID: {args.TileId}");
            Logger.Current.Info($"Previous state: {args.PreviousExecutionState}");

            await InitializeApi();
            await InitializeBackgroundTasks();
            InitializeTitleBar();
            await InitializeStatusBar();

            ShowContent();

            stopwatch.Stop();
            Logger.Current.Info($"Launch time: {stopwatch.Elapsed}");
        }

        private Task OnSuspendingAsync(DateTimeOffset deadline)
        {
            Logger.Current.Info($"Suspending (deadline: {deadline.DateTime.ToString(Logger.Current.DateTimeFormat)})");

            var api = ViewModelLocator.Container.Resolve<IApi>();
#warning Ukládání nastavení
            //await api.SaveSettingsAsync();

            return Task.CompletedTask;
        }

        private void OnResuming(object data)
        {

        }

        private void OnUnhandledException(UnhandledExceptionEventArgs args)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else
            {
                Logger.Current.Fatal($"Unhandled exception: {args.Message}", args.Exception);
            }
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(480, 500));
        }

        protected async override void OnBackgroundActivated(BackgroundActivatedEventArgs args)
        {
            await InitializeLogger();

            var taskInstance = args.TaskInstance;
            Logger.Current.Info($"Background task '{taskInstance.Task.Name}' activated");
            switch (taskInstance.Task.Name)
            {
            case InProcessTestBackgroundTask.Name:
                BackgroundTaskService.Run<InProcessTestBackgroundTask>(taskInstance);
                break;
            default:
                Logger.Current.Warn($"Unknown background task '{taskInstance.Task.Name}'");
                break;
            }
        }


        private async Task InitializeLogger()
        {
            if (Logger.Current is NullLogger)
            {
#if DEBUG
                var folder = ApplicationData.Current.LocalFolder;
                var file = await folder.CreateFileAsync("Log.txt", CreationCollisionOption.OpenIfExists);
                var logger = new FileLogger(file.Path);
                Logger.Current = logger;
                //Logger.Current = Debugger.IsAttached
                //    ? (ILogger)new DebugLogger()
                //    : (ILogger)new MemoryLogger();
#else
                // Logger v releasu zatím nepoužíváme - výchozí je NullLogger.
#endif
            }
        }

        private async Task InitializeBackgroundTasks()
        {
            var backgroundTaskService = ViewModelLocator.Container.Resolve<IBackgroundTaskService>();
            await backgroundTaskService.InitializeAsync();
            if (backgroundTaskService.IsInitialized)
            {
                backgroundTaskService.TryRegister<InProcessTestBackgroundTask>(new ToastNotificationHistoryChangedTrigger());
                Logger.Current.Info("Background tasks initialized");
            }
            else
            {
                Logger.Current.Info("Cannot initialize background tasks");
            }
        }

        private async Task InitializeApi()
        {
            var api = ViewModelLocator.Container.Resolve<IApi>();
#warning Inicializace dat a nastavení
            //await api.LoadSettingsAsync();
            //await api.InitializeDataAsync();

#if DEBUG
            var folders = await api.FolderQuery.GetAll();
            if (!folders.Any())
            {
                await api.SavePreviewData();
            }
#else
            return Task.CompletedTask;
#endif

            Logger.Current.Info("Data initialized");
        }

        private void InitializeTitleBar()
        {
            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = false;
        }

        private async Task InitializeStatusBar()
        {
            if (ApiInformationHelper.Check(ApiContract.Phone, 1, 0))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.ForegroundColor = (Color)Resources["SystemChromeAltLowColor"];
                    statusBar.BackgroundColor = (Color)Resources["SystemChromeLowColor"];
                    statusBar.BackgroundOpacity = 1;
                    await statusBar.ShowAsync();
                }
            }
        }


        private void ShowContent()
        {
            var content = new MainPage();
            Window.Current.Content = content;
            Window.Current.Activate();
        }
    }
}
