using OneDo.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Services.NavigationService
{
    public interface INavigationService
    {
        Frame Frame { get; }

        Type SourcePageType { get; set; }

        Type CurrentSourcePageType { get; }

        bool CanGoForward { get; }

        bool CanGoBack { get; }

        IList<PageStackEntry> BackStack { get; }

        IList<PageStackEntry> ForwardStack { get; }

        bool Navigate<TBasePage>() where TBasePage : BasePage;

        bool Navigate<TBasePage>(object parameter) where TBasePage : BasePage;

        bool Navigate(Type basePageType);

        bool Navigate(Type basePageType, object parameter);

        void GoForward();

        void GoBack();
    }
}
