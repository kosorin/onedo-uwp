using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using OneDo.ViewModel;
using OneDo.View;
using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Services.NavigationService
{
    public class NavigationService : ViewModelBase, INavigationService
    {
        public Frame Frame { get; }

        private ModalViewModel modal;
        public ModalViewModel Modal
        {
            get { return modal; }
            private set
            {
                if (Set(ref modal, value))
                {
                    UpdateBackButtonVisibility();
                }
            }
        }

        public bool CanGoForward => Modal == null && Frame.CanGoForward;

        public bool CanGoBack => Modal != null || Frame.CanGoBack;


        public ICommand GoForwardCommand { get; }

        public ICommand GoBackCommand { get; }


        public NavigationService()
        {
            // Dummy constructor
        }

        public NavigationService(Window window)
        {
            Frame = (window.Content as Frame) ?? new Frame();

            if (!DesignMode.DesignModeEnabled)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackButtonRequested;
            }
            Frame.NavigationFailed += OnNavigationFailed;
            Frame.Navigating += OnNavigating;
            Frame.Navigated += OnNavigated;

            GoForwardCommand = new RelayCommand(TryGoForward);
            GoBackCommand = new RelayCommand(TryGoBack);
        }


        public bool Navigate<TPageBase>() where TPageBase : ExtendedPage
        {
            return Navigate(typeof(TPageBase));
        }

        public bool Navigate<TPageBase>(object parameter) where TPageBase : ExtendedPage
        {
            return Navigate(typeof(TPageBase), parameter);
        }

        public bool Navigate(Type pageType)
        {
            CloseModal();
            return Frame.Navigate(pageType);
        }

        public bool Navigate(Type pageType, object parameter)
        {
            CloseModal();
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
            CloseModal();
            Frame.GoForward();
        }

        public void TryGoForward()
        {
            if (CanGoForward)
            {
                GoForward();
            }
        }

        public void GoBack()
        {
            if (Modal != null)
            {
                CloseModal();
            }
            else
            {
                Frame.GoBack();
            }
        }

        public void TryGoBack()
        {
            if (CanGoBack)
            {
                GoBack();
            }
        }

        public void ShowModal(ModalViewModel modal)
        {
            Modal = modal;
        }

        public void CloseModal()
        {
            Modal = null;
        }


        private INavigable GetContext()
        {
            return (Frame.Content as ExtendedPage)?.DataContext as INavigable;
        }

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
    }
}
