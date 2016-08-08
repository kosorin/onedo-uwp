using OneDo.ViewModels;
using OneDo.Views;
using System;
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


        bool Navigate<TPageBase>() where TPageBase : PageBase;

        bool Navigate<TPageBase>(object parameter) where TPageBase : PageBase;

        bool Navigate(Type pageType);

        bool Navigate(Type pageType, object parameter);

        void ShowFlyout(FlyoutViewModel flyout);

        void ClearHistory();

        void GoForward();

        void GoBack();
    }
}
