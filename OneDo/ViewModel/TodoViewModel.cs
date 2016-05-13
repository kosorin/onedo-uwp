using OneDo.Services.NavigationService;

namespace OneDo.ViewModel
{
    public class TodoViewModel : PageViewModel
    {
        public TodoViewModel(INavigationService navigationService)
            : base(navigationService)
        {

        }
    }
}