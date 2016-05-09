using GalaSoft.MvvmLight;
using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace OneDo.ViewModel.Pages
{
    public class MainPageViewModel : BasePageViewModel
    {
        private string text = "Before";
        public string Text
        {
            get { return text; }
            set { Set(ref text, value); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { Set(ref isBusy, value); }
        }

        public ICommand TestCommand { get; }

        public IDataService DataService { get; }

        public MainPageViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService)
        {
            DataService = dataService;
            DataService.Loaded += DataService_Loaded;

            IsBusy = true;
            DataService.LoadAsync();
        }

        private void DataService_Loaded(object sender, EventArgs e)
        {
            IsBusy = false;
            Text = "After";
        }
    }
}
