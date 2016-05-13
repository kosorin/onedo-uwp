using Microsoft.Practices.ServiceLocation;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
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
            InitializeLocator();
            RegisterEventHandlers();

#if DEBUG
            ApplicationView.PreferredLaunchViewSize = new Size(480, 800);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
#endif
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            ShowSplashScreen(args);
            InitializeNavigation();
            await LoadDataAsync();
            ShowStartPage();

            stopwatch.Stop();
            Debug.WriteLine($"Start-up time: {stopwatch.Elapsed}");
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


        private void RegisterEventHandlers()
        {
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
        }

        private void InitializeLocator()
        {
            RuntimeHelpers.RunClassConstructor(typeof(ViewModelLocator).TypeHandle);
        }

        private void InitializeNavigation()
        {
            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.Initialize(Window.Current);
        }

        private void ShowSplashScreen(LaunchActivatedEventArgs args)
        {
            Window.Current.Content = new SplashScreenPage(args.SplashScreen);
            Window.Current.Activate();
        }

        private void ShowStartPage()
        {
            var navigationService = ServiceLocator.Current.GetInstance<INavigationService>();
            navigationService.Navigate(StartPageType);
            Window.Current.Content = navigationService.Frame;
        }

        private async Task LoadDataAsync()
        {
            var dataService = ServiceLocator.Current.GetInstance<IDataService>();
            await dataService.LoadAsync();
        }
    }
}
