using OneDo.Services.NavigationService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Views;
using System.Threading.Tasks;
using OneDo.Model.Data;
using System.Collections.Generic;
using OneDo.Model.Data.Objects;

namespace OneDo.ViewModels
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


        private List<Todo> todos;
        public List<Todo> Todos
        {
            get { return todos; }
            set { Set(ref todos, value); }
        }


        public ICommand TogglePaneCommand { get; }

        public ICommand NavigateToAboutPageCommand { get; }

        public ICommand NavigateToSettingsPageCommand { get; }

        public MainViewModel(INavigationService navigationService, IDataProvider dataProvider)
            : base(navigationService, dataProvider)
        {
            TogglePaneCommand = new RelayCommand(() => IsPaneOpen = !IsPaneOpen);
            NavigateToAboutPageCommand = new RelayCommand(() => NavigationService.Navigate<AboutPage>());
            NavigateToSettingsPageCommand = new RelayCommand(() => NavigationService.Navigate<SettingsPage>());

            Todos = dataProvider.Todos;
        }


        public override Task OnNavigatedFromAsync()
        {
            IsPaneOpen = false;
            return Task.CompletedTask;
        }
    }
}
