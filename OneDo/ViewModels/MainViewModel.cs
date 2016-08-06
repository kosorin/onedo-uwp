using OneDo.Services.NavigationService;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using OneDo.Services.Context;
using OneDo.ViewModels.Items;
using OneDo.ViewModels.Editors;
using OneDo.Model.Data.Repositories;

namespace OneDo.ViewModels.Pages
{
    public class MainViewModel : PageViewModel
    {
        private TodoEditorViewModel todoEditor;
        public TodoEditorViewModel TodoEditor
        {
            get { return todoEditor; }
            set
            {
                if (Set(ref todoEditor, value))
                {
                    RaisePropertyChanged(nameof(IsTodoEditorOpen));
                }
            }
        }

        public bool IsTodoEditorOpen
        {
            get { return TodoEditor != null; }
            set
            {
                if (!value)
                {
                    TodoEditor = null;
                }
            }
        }

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
            LoadData();
        }

        private void OnTodoItemSelected(TodoItemViewModel todoItem)
        {
            Context.TodoId = todoItem.Id;
            TodoEditor = new TodoEditorViewModel(DataProvider, Context);
        }
    }
}
