using GalaSoft.MvvmLight.Ioc;
using OneDo.Common.Logging;
using OneDo.Model.DataAccess;
using OneDo.Services.NavigationService;
using OneDo.Views;
using OneDo.ViewModels;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace OneDo
{
    sealed partial class App : Application
    {
        public Type StartPageType => typeof(MainPage);


        private readonly Stopwatch stopwatch = new Stopwatch();

        public App()
        {
            stopwatch.Start();

            InitializeComponent();
            InitializeLogger();
            InitializeLocator();

            Suspending += async (s, e) =>
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
            ShowSplashScreen(args.SplashScreen);
            await InitializeData();
            InitializeNavigation();
            ShowStartPage();

            stopwatch.Stop();
            Logger.Current.Info($"Start-up time: {stopwatch.Elapsed}");
        }

        private Task OnSuspendingAsync(DateTimeOffset deadline)
        {
            // TODO: Save application state and stop any background activity
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
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
#if DEBUG
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(200, 200));
#endif
        }


        private void InitializeLogger()
        {
            ILogger logger;
#if DEBUG
            logger = new DebugLogger();
#else
            logger = new NullLogger();
#endif
            Logger.Set(logger);
        }

        private void InitializeLocator()
        {
            RuntimeHelpers.RunClassConstructor(typeof(ViewModelLocator).TypeHandle);
        }

        private void InitializeNavigation()
        {
            var navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            navigationService.Initialize(Window.Current);
        }

        private async Task InitializeData()
        {
            var dataProvider = SimpleIoc.Default.GetInstance<IDataProvider>();
            await dataProvider.LoadAsync();
        }


        private void ShowSplashScreen(SplashScreen splashScreen)
        {
            Window.Current.Content = new SplashScreenPage(splashScreen);
            Window.Current.Activate();
        }

        private void ShowStartPage()
        {
            var navigationService = SimpleIoc.Default.GetInstance<INavigationService>();
            navigationService.Navigate(StartPageType);
            Window.Current.Content = navigationService.Frame;
        }
    }
}
