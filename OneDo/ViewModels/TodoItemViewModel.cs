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
        public Guid Id => todo.Id;

        public string Title => todo.Title;


        public Todo Object { get; set; }

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
