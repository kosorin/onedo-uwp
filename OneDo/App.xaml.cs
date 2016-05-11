using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using OneDo.View;
using OneDo.View.Pages;
using OneDo.ViewModel;
using OneDo.ViewModel.Pages;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneDo
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeLocator();
            RegisterEventHandlers();
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var shell = InitializeShell(args.PreviousExecutionState == ApplicationExecutionState.Terminated);

            Window.Current.Content = new SplashScreenPage(args.SplashScreen);
            Window.Current.Activate();

            await LoadDataAsync();

            Window.Current.Content = shell;

            ServiceLocator.Current.GetInstance<INavigationService>().Navigate<MainPage>();
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
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Služby
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }
            SimpleIoc.Default.Register<INavigationService, ShellNavigationService>();

            // Shell
            SimpleIoc.Default.Register<ShellViewModel>();

            // Stránky
            SimpleIoc.Default.Register<StartPageViewModel>();
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<AboutPageViewModel>();
        }

        private Shell InitializeShell(bool loadState)
        {
            var shell = Window.Current.Content as Shell;
            if (shell == null)
            {
                shell = new Shell();

                if (loadState)
                {
                    // TODO: Load state from previously suspended application
                }
            }
            return shell;
        }

        private async Task LoadDataAsync()
        {
            var dataService = ServiceLocator.Current.GetInstance<IDataService>();
            await dataService.LoadAsync();
        }
    }
}
