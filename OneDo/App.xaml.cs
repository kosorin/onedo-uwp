﻿using GalaSoft.MvvmLight.Ioc;
using Microsoft.EntityFrameworkCore;
using OneDo.Common.Logging;
using OneDo.Model.Data;
using OneDo.Services.ModalService;
using OneDo.View;
using OneDo.ViewModel;
using System;
using System.Linq;
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
using OneDo.Model.Data.Entities;
using GalaSoft.MvvmLight.Messaging;
using Windows.System;
using Windows.UI.Xaml.Media;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI;
using OneDo.Common.Metadata;

namespace OneDo
{
    sealed partial class App : Application
    {
        static App()
        {
            RuntimeHelpers.RunClassConstructor(typeof(ViewModelLocator).TypeHandle);
        }

        public App()
        {
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

            InitializeTitleBar();
            InitializeLogger();

            Logger.Current.Info($"Arguments: \"{args.Arguments}\"");
            Logger.Current.Info($"Launch reason: {args.Kind}");
            Logger.Current.Info($"Tile ID: {args.TileId}");
            Logger.Current.Info($"Previous state: {args.PreviousExecutionState}");

            ShowSplashScreen(args.SplashScreen);

            await InitializeSettings();
            await InitializeData();
            await InitializeStatusBar();
            InitializeModalService();

            ShowContent();

            stopwatch.Stop();
            Logger.Current.Info($"Launch time: {stopwatch.Elapsed}");
        }

        private async Task OnSuspendingAsync(DateTimeOffset deadline)
        {
            Logger.Current.Info($"Suspending (deadline: {deadline.DateTime.ToString(Logger.Current.DateTimeFormat)})");
            // TODO: Save application state and stop any background activity

            var settingsProvider = ViewModelLocator.Container.Resolve<ISettingsProvider>();
            await settingsProvider.SaveAsync();
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
                Logger.Current.Fatal("Unhandled exception", args.Exception);
            }
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
#if DEBUG
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(200, 200));
#endif
        }


        private void InitializeTitleBar()
        {
            var titleBar = CoreApplication.GetCurrentView().TitleBar;
            titleBar.ExtendViewIntoTitleBar = false;
        }

        private void InitializeLogger()
        {
#if DEBUG
            //var folder = ApplicationData.Current.LocalFolder;
            //var file = await folder.CreateFileAsync("OneDo.Log.txt", CreationCollisionOption.OpenIfExists);
            //var logger = new FileLogger(file.Path);
            //Logger.Current = logger;
            Logger.Current = Debugger.IsAttached
                ? (ILogger)new DebugLogger()
                : (ILogger)new MemoryLogger();
#else
            // Logger v releasu zatím nepoužíváme - výchozí je NullLogger.
#endif
            Logger.Current.Line("=================================================");
            Logger.Current.Line("===================== OneDo =====================");
            Logger.Current.Line("=================================================");
            Logger.Current.Info("Logger initialized");
        }

        private async Task InitializeSettings()
        {
            ShowSplashScreenText("Loading settings...");

            var settingsProvider = ViewModelLocator.Container.Resolve<ISettingsProvider>();
            await settingsProvider.LoadAsync();
            Logger.Current.Info("Settings initialized");
        }

        private async Task InitializeData()
        {
            ShowSplashScreenText("Initializing data...");

            using (var dc = new DataContext())
            {
                await dc.Database.MigrateAsync();
            }
            Logger.Current.Info("Data initialized");
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

        private void InitializeModalService()
        {
            ShowSplashScreenText(null);

            ViewModelLocator.Container.Resolve<IModalService>(
                TypedParameter.From(Window.Current),
                TypedParameter.From(SystemNavigationManager.GetForCurrentView()));

            Logger.Current.Info("Modal service initialized");
        }


        private void ShowSplashScreen(SplashScreen splashScreen)
        {
            Window.Current.Content = new SplashScreenPage(splashScreen);
            Window.Current.Activate();
        }

        private void ShowContent()
        {
            ShowSplashScreenText(null);

            var content = new MainPage();
            Window.Current.Content = content;
        }


        private void ShowSplashScreenText(string text)
        {
            Messenger.Default.Send(new SplashScreenMessage(text));
        }
    }
}
