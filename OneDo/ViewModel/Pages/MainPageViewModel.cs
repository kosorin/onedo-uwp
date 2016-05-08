using GalaSoft.MvvmLight;
using OneDo.Model;
using OneDo.Model.Recurrences;
using OneDo.Services.DataService;
using System.Collections.Generic;

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
