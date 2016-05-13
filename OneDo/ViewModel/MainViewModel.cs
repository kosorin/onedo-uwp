using OneDo.Services.DataService;
using OneDo.Services.NavigationService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.View;

namespace OneDo.ViewModel
{
    public class MainViewModel : PageViewModel
    {
        private bool isPaneOpen;
        public bool IsPaneOpen
        {
            get { return KeepPaneOpen || isPaneOpen; }
            set { Set(ref isPaneOpen, value); }
        }

        private bool keepPaneOpen;
        public bool KeepPaneOpen
        {
            get { return keepPaneOpen; }
            set
            {
                if (Set(ref keepPaneOpen, value))
                {
                    RaisePropertyChanged(nameof(IsPaneOpen));
                }
            }
        }


        public ICommand TogglePaneCommand { get; }

        public ICommand NavigateToMainPageCommand { get; }

        public ICommand NavigateToAboutPageCommand { get; }

        public IDataService DataService { get; }

        public MainViewModel(INavigationService navigationService, IDataService dataService)
            : base(navigationService)
        {
            DataService = dataService;

            TogglePaneCommand = new RelayCommand(() => IsPaneOpen = !IsPaneOpen);
            NavigateToMainPageCommand = new RelayCommand(() => Navigate<MainPage>());
            NavigateToAboutPageCommand = new RelayCommand(() => Navigate<AboutPage>());
        }


        private void Navigate<TPageBase>() where TPageBase : PageBase
        {
            NavigationService.Navigate<TPageBase>();
            IsPaneOpen = false;
        }
    }
}
