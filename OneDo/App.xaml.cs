using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
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
            RegisterEventHandlers();
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

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
//#if DEBUG
//            if (Debugger.IsAttached)
//            {
//                DebugSettings.EnableFrameRateCounter = true;
//            }
//#endif

            var shell = Window.Current.Content as Shell;
            if (shell == null)
            {
                shell = new Shell();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                Window.Current.Content = shell;
            }
            Window.Current.Activate();
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
    }
}
