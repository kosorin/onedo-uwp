using OneDo.Model.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModels
{
    public class TodoEventArgs : EventArgs
    {
        public Todo Todo { get; set; }

        public TodoEventArgs(Todo todo)
        {
            Todo = todo;
        }
    }
}
