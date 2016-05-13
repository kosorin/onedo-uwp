using OneDo.Services.NavigationService;
using Windows.ApplicationModel;

namespace OneDo.ViewModel
{
    public class SettingsViewModel : PageViewModel
    {
        public SettingsViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }
    }
}