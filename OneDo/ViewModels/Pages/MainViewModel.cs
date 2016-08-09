using OneDo.Services.NavigationService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using OneDo.ViewModels.Items;
using OneDo.ViewModels.Flyouts;
using OneDo.Model.Data.Repositories;
using Windows.UI.Core;
using OneDo.Model.Data.Objects;
using System.Collections.ObjectModel;

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
            ResetDataCommand = new RelayCommand(ResetData);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            LoadData();
        }


        private void LoadData()
        {
            TodoItems = new ObservableCollection<TodoItemViewModel>(DataProvider
                .Todos
                .GetAll()
                .Select(t => new TodoItemViewModel(t)));
        }

        private void ResetData()
        {
            var todos = new DesignDataProvider()
                .Todos
                .GetAll();

            DataProvider.Todos.RemoveAll();
            DataProvider.Todos.AddAll(todos);
            LoadData();
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
