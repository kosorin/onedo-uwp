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

        public ICommand NavigateToAboutPageCommand { get; }

        public ICommand NavigateToSettingsPageCommand { get; }

        public MainViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            TogglePaneCommand = new RelayCommand(() => IsPaneOpen = !IsPaneOpen);
            NavigateToAboutPageCommand = new RelayCommand(() => Navigate<AboutPage>());
            NavigateToSettingsPageCommand = new RelayCommand(() => Navigate<SettingsPage>());
        }


        private void Navigate<TPageBase>() where TPageBase : PageBase
        {
            NavigationService.Navigate<TPageBase>();
            IsPaneOpen = false;
        }
    }
}
