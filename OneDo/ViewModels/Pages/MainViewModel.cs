using OneDo.Services.NavigationService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using OneDo.Services.Context;
using OneDo.ViewModels.Items;
using OneDo.ViewModels.Flyouts;
using OneDo.Model.Data.Repositories;
using Windows.UI.Core;

namespace OneDo.ViewModels.Pages
{
    public class MainViewModel : PageViewModel
    {
        private List<TodoItemViewModel> todoItems;
        public List<TodoItemViewModel> TodoItems
        {
            get { return todoItems; }
            set { Set(ref todoItems, value); }
        }


        public ICommand TodoItemSelectedCommand { get; }

        public ICommand ResetDataCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        public MainViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context)
            : base(navigationService, dataProvider, context)
        {
            TodoItemSelectedCommand = new RelayCommand<TodoItemViewModel>(OnTodoItemSelected);
            ResetDataCommand = new RelayCommand(ResetData);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            LoadData();
        }


        private void LoadData()
        {
            TodoItems = DataProvider
                .Todos
                .GetAll()
                .Select(t => new TodoItemViewModel(t))
                .ToList();
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

        private void OnTodoItemSelected(TodoItemViewModel todoItem)
        {
            Context.TodoId = todoItem.Id;

            var editor = new TodoEditorViewModel(NavigationService, DataProvider, Context);
            editor.Saved += (s, e) => LoadData();
            NavigationService.ShowFlyout(editor);
        }

        private void ShowSettings()
        {
            NavigationService.ShowFlyout(new SettingsViewModel(NavigationService, DataProvider, Context));
        }
    }
}
