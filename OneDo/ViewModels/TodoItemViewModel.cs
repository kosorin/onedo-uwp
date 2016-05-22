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
        public string Id => todo.Title;

        public string Title => todo.Title;


        private readonly Todo todo;

        public TodoItemViewModel(Todo todo)
        {
            if (todo == null)
            {
                throw new ArgumentNullException(nameof(todo));
            }
            this.todo = todo;
        }
    }
}
