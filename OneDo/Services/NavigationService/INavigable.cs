using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace OneDo.Services.NavigationService
{
    public interface INavigable
    {
        void OnNavigatingFrom(NavigatingEventArgs args);

        Task OnNavigatedFromAsync();

        void OnNavigatedTo(object parameter, NavigationMode mode);

        void OnBackButton(BackButtonEventArgs args);

        void OnFlyoutClosed(FlyoutClosedEventArgs args);
    }
}