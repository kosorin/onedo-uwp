using OneDo.View;
using OneDo.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Services.NavigationService
{
    public class ShellNavigationService : INavigationService
    {
        public Frame Frame { get; } = new Frame();

        private INavigable ViewModel => (Frame.Content as BasePage)?.DataContext as INavigable;

        public ShellNavigationService()
        {
            if (!DesignMode.DesignModeEnabled)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackButtonRequested;
            }

            Frame.Navigating += OnNavigating;
            Frame.Navigated += OnNavigated;
            Navigate(typeof(MainPage));
        }

        private async void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            var viewModel = ViewModel;
            if (viewModel != null)
            {
                var args = new NavigatingEventArgs();
                viewModel.OnNavigatingFrom(args);
                e.Cancel = args.Handled;
                if (!e.Cancel)
                {
                    await viewModel.OnNavigatedFromAsync();
                }
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            UpdateBackButtonVisibility();

            ViewModel?.OnNavigatedTo(e.Parameter, e.NavigationMode);
        }

        private void OnBackButtonRequested(object sender, BackRequestedEventArgs e)
        {
            var args = new BackButtonEventArgs { Handled = e.Handled };

            ViewModel?.OnBackButton(args);

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


        public Type SourcePageType
        {
            get { return Frame.SourcePageType; }
            set { Frame.SourcePageType = value; }
        }

        public Type CurrentSourcePageType => Frame.CurrentSourcePageType;

        public bool Navigate<TBasePage>() where TBasePage : BasePage
        {
            return Frame.Navigate(typeof(TBasePage));
        }

        public bool Navigate<TBasePage>(object parameter) where TBasePage : BasePage
        {
            return Frame.Navigate(typeof(TBasePage), parameter);
        }

        public bool Navigate(Type basePageType)
        {
            return Frame.Navigate(basePageType);
        }

        public bool Navigate(Type basePageType, object parameter)
        {
            return Frame.Navigate(basePageType, parameter);
        }

        public void GoForward()
        {
            Frame.GoForward();
        }

        public void GoBack()
        {
            Frame.GoBack();
        }

        public bool CanGoForward => Frame.CanGoForward;

        public bool CanGoBack => Frame.CanGoBack;

        public IList<PageStackEntry> BackStack => Frame.BackStack;

        public IList<PageStackEntry> ForwardStack => Frame.ForwardStack;
    }
}
