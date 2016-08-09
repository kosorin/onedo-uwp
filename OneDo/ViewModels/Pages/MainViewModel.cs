using OneDo.Services.NavigationService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using OneDo.ViewModels.Items;
using OneDo.ViewModels.Flyouts;
using Windows.UI.Core;
using OneDo.Model.Data.Objects;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OneDo.ViewModels.Commands;

namespace OneDo.ViewModels.Pages
{
    public class MainViewModel : PageViewModel
    {
        private ObservableCollection<TodoItemViewModel> todoItems;
        public ObservableCollection<TodoItemViewModel> TodoItems
        {
            get { return todoItems; }
            set { Set(ref todoItems, value); }
        }

        private TodoItemViewModel selectedTodoItem;
        public TodoItemViewModel SelectedTodoItem
        {
            get { return selectedTodoItem; }
            set { Set(ref selectedTodoItem, value); }
        }


        public ICommand TodoItemTappedCommand { get; }

        public ICommand AddTodoCommand { get; }

        public ICommand ResetDataCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        public MainViewModel(INavigationService navigationService, IDataProvider dataProvider)
            : base(navigationService, dataProvider)
        {
            TodoItemTappedCommand = new RelayCommand(TodoItemTapped);
            AddTodoCommand = new RelayCommand(AddTodo);
            ResetDataCommand = new AsyncRelayCommand(ResetData);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            LoadData();
        }

        private async Task LoadData()
        {
            var todos = await DataProvider.Context
               .Todos
               .ToAsyncEnumerable()
               .ToList();
            var todoItems = todos.Select(t => new TodoItemViewModel(t));
            TodoItems = new ObservableCollection<TodoItemViewModel>(todoItems);
        }

        private async Task ResetData()
        {
            DataProvider.Context.Todos.RemoveRange(TodoItems.Select(x => x.Todo));
            await DataProvider.Context.SaveChangesAsync();
            TodoItems.Clear();
            await Task.Delay(2000);
        }

        private void AddTodo()
        {
            var editor = new TodoEditorViewModel(NavigationService, DataProvider, null);
            editor.Saved += (s, e) =>
            {
                TodoItems.Add(new TodoItemViewModel(e.Todo));
            };
            ShowTodoEditor(editor);
        }

        private void TodoItemTapped()
        {
            if (SelectedTodoItem != null)
            {
                var editor = new TodoEditorViewModel(NavigationService, DataProvider, SelectedTodoItem.Todo);
                editor.Saved += (s, e) =>
                {
                    SelectedTodoItem.Refresh();
                };
                ShowTodoEditor(editor);
            }
        }

        private void ShowTodoEditor(TodoEditorViewModel editor)
        {
            editor.Saved += (s, e) => NavigationService.CloseFlyout();
            NavigationService.ShowFlyout(editor);
        }

        private void ShowSettings()
        {
            NavigationService.ShowFlyout(new SettingsViewModel(NavigationService, DataProvider));
        }
    }
}
