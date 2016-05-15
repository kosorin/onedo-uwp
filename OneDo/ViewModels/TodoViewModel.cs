using OneDo.Services.NavigationService;

namespace OneDo.ViewModels
{
    public class TodoViewModel : PageViewModel
    {
        public TodoViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }
    }
}