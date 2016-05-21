using GalaSoft.MvvmLight;
using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModels
{
    public class TodoItemViewModel : ViewModelBase
    {
        public string Title => Todo.Title;


        public Todo Todo { get; }

        public TodoItemViewModel(Todo todo)
        {
            Todo = todo;
        }
    }
}
