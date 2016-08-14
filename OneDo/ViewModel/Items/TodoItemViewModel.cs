using GalaSoft.MvvmLight;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Items
{
    public class TodoItemViewModel : ExtendedViewModel
    {
        public bool IsCompleted => Todo.Completed != null;

        public string Title => Todo.Title;


        public Todo Todo { get; }

        public TodoItemViewModel(Todo todo)
        {
            Todo = todo;
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(IsCompleted));
            RaisePropertyChanged(nameof(Title));
        }
    }
}
