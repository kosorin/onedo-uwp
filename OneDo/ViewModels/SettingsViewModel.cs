using OneDo.Model.Data;
using OneDo.Services.NavigationService;

namespace OneDo.ViewModels
{
    public class SettingsViewModel : PageViewModel
    {
        public SettingsViewModel(INavigationService navigationService, IDataProvider dataProvider)
            : base(navigationService, dataProvider)
        {

        }
    }
}