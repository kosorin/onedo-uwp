using GalaSoft.MvvmLight.Ioc;
using OneDo.Common.Logging;
using OneDo.Model.Data;
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
using Windows.Storage;
using Autofac;
using OneDo.Views.Pages;
using Windows.UI.Core;

namespace OneDo
{
    sealed partial class App : Application
    {
        public Type StartPageType => typeof(MainPage);


        private readonly Stopwatch stopwatch = new Stopwatch();

        static App()
        {
            RuntimeHelpers.RunClassConstructor(typeof(ViewModelLocator).TypeHandle);
        }

        public App()
        {
            stopwatch.Start();

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
            await InitializeLogger();

            Logger.Current.Info($"Arguments: \"{args.Arguments}\"");
            Logger.Current.Info($"Launch reason: {args.Kind}");
            Logger.Current.Info($"Previous state: {args.PreviousExecutionState}");

            ShowSplashScreen(args.SplashScreen);

            await InitializeData();
            InitializeNavigation();

            ShowStartPage();

            stopwatch.Stop();
            Logger.Current.Info($"Start-up time: {stopwatch.Elapsed}");
        }

        private async Task OnSuspendingAsync(DateTimeOffset deadline)
        {
            Logger.Current.Info($"Suspending (deadline: {deadline.DateTime.ToString(Logger.Current.DateTimeFormat)})");
            // TODO: Save application state and stop any background activity

            var dataProvider = ViewModelLocator.Container.Resolve<IDataProvider>();
            await dataProvider.SaveAsync();
        }

        private void OnResuming(object data)
        {
            Logger.Current.Info("Resuming");
        }

        private void OnUnhandledException(UnhandledExceptionEventArgs args)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
            else
            {
                Logger.Current.Fatal("Unhandled exception", args.Exception);
            }
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
#if DEBUG
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(200, 200));
#endif
        }


        private async Task InitializeLogger()
        {
#if DEBUG
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync("Log.txt", CreationCollisionOption.OpenIfExists);
            var logger = new FileLogger(file.Path);
            Logger.Current = logger;
#else
            // Logger v releasu zatím nepoužíváme.
#endif
            Logger.Current.Line("=================================================");
            Logger.Current.Line("===================== OneDo =====================");
            Logger.Current.Line("=================================================");
            Logger.Current.Info("Logger initialized");
        }

        private void InitializeNavigation()
        {
            var navigationService = ViewModelLocator.Container.Resolve<INavigationService>(TypedParameter.From(Window.Current));
            Window.Current.CoreWindow.PointerPressed += (sender, args) =>
            {
                try
                {
                    var pointer = args.CurrentPoint;
                    if (pointer.Properties.IsXButton1Pressed)
                    {
                        navigationService.TryGoBack();
                        args.Handled = true;
                    }
                }
                catch (Exception e)
                {
                    Logger.Current.Debug("PointerPressed", e);
                }
            };
            Logger.Current.Info("Navigation initialized");
        }

        private async Task InitializeData()
        {
            var dataProvider = ViewModelLocator.Container.Resolve<IDataProvider>();
            await dataProvider.LoadAsync();
            Logger.Current.Info("Data initialized");
        }


        private void ShowSplashScreen(SplashScreen splashScreen)
        {
            Window.Current.Content = new SplashScreenPage(splashScreen);
            Window.Current.Activate();
        }

        private void ShowStartPage()
        {
            var navigationService = ViewModelLocator.Container.Resolve<INavigationService>();
            navigationService.Navigate(StartPageType);
            Window.Current.Content = navigationService.Frame;
        }
    }
}
