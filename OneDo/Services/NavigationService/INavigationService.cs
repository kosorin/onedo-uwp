using OneDo.ViewModels;
using OneDo.Views;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.Services.NavigationService
{
    public interface INavigationService
    {
        Frame Frame { get; }

        FlyoutViewModel Flyout { get; }

        bool CanGoForward { get; }

        bool CanGoBack { get; }


        ICommand GoForwardCommand { get; }

        ICommand GoBackCommand { get; }


        bool Navigate<TPageBase>() where TPageBase : ExtendedPage;

        bool Navigate<TPageBase>(object parameter) where TPageBase : ExtendedPage;

        bool Navigate(Type pageType);

        bool Navigate(Type pageType, object parameter);

        void ClearHistory();

        void GoForward();

        void TryGoForward();

        void GoBack();

        void TryGoBack();


        void ShowFlyout(FlyoutViewModel flyout);

        void CloseFlyout();
    }
}
