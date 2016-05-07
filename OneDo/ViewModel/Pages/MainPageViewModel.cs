using GalaSoft.MvvmLight;
using OneDo.Services.DataService;

namespace OneDo.ViewModel.Pages
{
    public class MainPageViewModel : ViewModelBase
    {
        public string Text { get; set; } = "Karel";

        public IDataService DataService { get; }

        public MainPageViewModel(IDataService dataService)
        {
            DataService = dataService;
            DataService.LoadAsync();
        }
    }
}
