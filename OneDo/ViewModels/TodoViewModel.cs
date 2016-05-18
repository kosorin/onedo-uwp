using OneDo.Model.Data;
using OneDo.Services.NavigationService;

namespace OneDo.ViewModels
{
    public class TodoViewModel : PageViewModel
    {
        public TodoViewModel(INavigationService navigationService, IDataProvider dataProvider)
            : base(navigationService, dataProvider)
        {

        }
    }
}