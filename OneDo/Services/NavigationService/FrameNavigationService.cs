using OneDo.Views;
using System;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Services.NavigationService
{
    public class FrameNavigationService : INavigationService
    {
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"Failed to load page {e.SourcePageType.FullName}");
        }

        private async void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var context = GetContext();
            if (context != null)
            {
                var args = new NavigatingEventArgs();
                context.OnNavigatingFrom(args);
                e.Cancel = args.Cancel;
                if (!e.Cancel)
                {
                    await context.OnNavigatedFromAsync();
                }
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            UpdateBackButtonVisibility();
            GetContext()?.OnNavigatedTo(e.Parameter, e.NavigationMode);
        }

        private void OnBackButtonRequested(object sender, BackRequestedEventArgs e)
        {
            var args = new BackButtonEventArgs { Handled = e.Handled };

            GetContext()?.OnBackButton(args);

            if (!args.Handled)
            {
                if (CanGoBack)
                {
                    e.Handled = true;
                    GoBack();
                }
            }
            else
            {
                e.Handled = true;
            }
        }

        private void UpdateBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
        }


        public bool IsInitialized { get; private set; } = false;

        public Frame Frame { get; } = null;

        public Type SourcePageType
        {
            get { return Frame.SourcePageType; }
            set { Frame.SourcePageType = value; }
        }

        public Type CurrentSourcePageType => Frame.CurrentSourcePageType;

        public bool CanGoForward => Frame.CanGoForward;

        public bool CanGoBack => Frame.CanGoBack;


        public FrameNavigationService()
        {
            // Dummy constructor
        }

        public FrameNavigationService(Window window)
        {
            Frame = (window.Content as Frame) ?? new Frame();

            if (!DesignMode.DesignModeEnabled)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackButtonRequested;
            }
            Frame.NavigationFailed += OnNavigationFailed;
            Frame.Navigating += OnNavigating;
            Frame.Navigated += OnNavigated;

            IsInitialized = true;
        }

        public bool Navigate<TPageBase>() where TPageBase : PageBase
        {
            return Navigate(typeof(TPageBase));
        }

        public bool Navigate<TPageBase>(object parameter) where TPageBase : PageBase
        {
            return Navigate(typeof(TPageBase), parameter);
        }

        public bool Navigate(Type pageType)
        {
            return Frame.Navigate(pageType);
        }

        public bool Navigate(Type pageType, object parameter)
        {
            return Frame.Navigate(pageType, parameter);
        }

        public void ClearHistory()
        {
            Frame.BackStack.Clear();
            Frame.ForwardStack.Clear();
            UpdateBackButtonVisibility();
        }

        public void GoForward()
        {
            Frame.GoForward();
        }

        public void GoBack()
        {
            Frame.GoBack();
        }


        private INavigable GetContext()
        {
            return (Frame.Content as PageBase)?.DataContext as INavigable;
        }
    }
}
