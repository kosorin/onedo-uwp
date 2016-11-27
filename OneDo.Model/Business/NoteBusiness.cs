using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;

namespace OneDo.Model.Business
{
    public class NoteBusiness : EntityBusiness<Note>
    {
        public NoteBusiness(DataService dataService) : base(dataService)
        {
        }
    }
}
