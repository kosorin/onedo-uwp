using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;

namespace OneDo.Model.Business
{
    public class TodoBusiness : BusinessBase
    {
        public TodoBusiness(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }

        public bool IsNew(Todo todo) => todo.Id == Guid.Empty;

        public Todo GetDefault()
        {
            return new Todo();
        }
    }
}
