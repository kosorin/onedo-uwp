using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;

namespace OneDo.Model.Business
{
    public class TodoBusiness : EntityBusiness<Todo>
    {
        public TodoBusiness(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }

        public override Todo Clone(Todo todo)
        {
            return todo;
        }

        public void ToggleComplete(Todo todo)
        {
            if (todo.Completed == null)
            {
                todo.Completed = DateTime.Now;
            }
            else
            {
                todo.Completed = null;
            }
        }
    }
}
