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

        public IContext Context { get; }

        public MainViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context)
            : base(navigationService, dataProvider)
        {
            Context = context;

            ResetDataCommand = new RelayCommand(ResetData);
            TodoItemSelectedCommand = new RelayCommand<TodoItemViewModel>(OnTodoItemSelected);

            LoadData();
        }


        private void LoadData()
        {
            TodoItems = DataProvider.Todos.GetAll().Select(t => new TodoItemViewModel(t)).ToList();
        }

        private void ResetData()
        {
            DataProvider.Todos.RemoveAll();
            DataProvider.Todos.AddAll(new DesignDataProvider().Todos.GetAll());
            LoadData();
        }

        private void OnTodoItemSelected(TodoItemViewModel todoItem)
        {
            Context.TodoId = todoItem.Id;
            NavigationService.ShowFlyout(new TodoEditorViewModel(DataProvider, Context));
        }
    }
}
