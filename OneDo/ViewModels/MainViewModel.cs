using OneDo.Services.NavigationService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Views;
using System.Threading.Tasks;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using OneDo.Model.Data.Objects;
using OneDo.Services.Context;

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


        private List<TodoItemViewModel> todoItems;
        public List<TodoItemViewModel> TodoItems
        {
            get { return todoItems; }
            set { Set(ref todoItems, value); }
        }


        public ICommand TogglePaneCommand { get; }

        public ICommand NavigateToAboutPageCommand { get; }

        public ICommand NavigateToSettingsPageCommand { get; }

        public ICommand TodoItemClickCommand { get; }

        public IContext Context { get; }

        public MainViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context)
            : base(navigationService, dataProvider)
        {
            Context = context;

            TogglePaneCommand = new RelayCommand(() => IsPaneOpen = !IsPaneOpen);
            NavigateToAboutPageCommand = new RelayCommand(() => NavigationService.Navigate<AboutPage>());
            NavigateToSettingsPageCommand = new RelayCommand(() => NavigationService.Navigate<SettingsPage>());
            TodoItemClickCommand = new RelayCommand<TodoItemViewModel>(OnTodoItemClick);

            TodoItems = DataProvider.Todos.Select(t => new TodoItemViewModel(t)).ToList();
        }

        public override Task OnNavigatedFromAsync()
        {
            IsPaneOpen = false;
            return Task.CompletedTask;
        }


        private void OnTodoItemClick(TodoItemViewModel todoItem)
        {
            Context.Todo = todoItem.Todo;
            NavigationService.Navigate<TodoPage>();
        }
    }
}
