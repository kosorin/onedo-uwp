using OneDo.Services.NavigationService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using OneDo.Services.Context;
using OneDo.ViewModels.Items;

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


        public ICommand TodoItemClickCommand { get; }

        public IContext Context { get; }

        public MainViewModel(INavigationService navigationService, IDataProvider dataProvider, IContext context)
            : base(navigationService, dataProvider)
        {
            Context = context;

            TodoItemClickCommand = new RelayCommand<TodoItemViewModel>(OnTodoItemClick);

            TodoItems = DataProvider.Todos.GetAll().Select(t => new TodoItemViewModel(t)).ToList();
        }


        private void OnTodoItemClick(TodoItemViewModel todoItem)
        {
            Context.TodoId = todoItem.Id;
        }
    }
}
