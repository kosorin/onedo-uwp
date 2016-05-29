using GalaSoft.MvvmLight;
using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModels
{
    public class TodoItemViewModel : ExtendedViewModelBase
    {
        public Guid Id => todo.Id;

        public string Title => todo.Title;


        private readonly Todo todo;

        public TodoItemViewModel(Todo todo)
        {
            this.todo = todo;
        }
    }
}
