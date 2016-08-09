using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Objects;

namespace OneDo.Model.Business
{
    public class TodoBusiness : BusinessBase
    {
        public TodoBusiness(IDataProvider dataProvider) : base(dataProvider)
        {

        }

        public bool IsNew(Todo todo) => todo.Id == Guid.Empty;

        public Todo GetDefault()
        {
            return new Todo();
        }
    }
}
