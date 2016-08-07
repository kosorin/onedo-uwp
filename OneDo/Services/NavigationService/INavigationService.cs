using OneDo.Views;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace OneDo.Services.NavigationService
{
    public interface INavigationService
    {
        bool IsInitialized { get; }

        Frame Frame { get; }

        Type SourcePageType { get; set; }

        Type CurrentSourcePageType { get; }

        bool CanGoForward { get; }

        bool CanGoBack { get; }


        bool Navigate<TPageBase>() where TPageBase : PageBase;

        bool Navigate<TPageBase>(object parameter) where TPageBase : PageBase;

        bool Navigate(Type pageType);

        bool Navigate(Type pageType, object parameter);

        void ClearHistory();

        void GoForward();

        void GoBack();
    }
}
