using OneDo.Model.Data;
using OneDo.Model.Entities;

namespace OneDo.Model.Business
{
    public class NoteBusiness : EntityBusiness<Note>
    {
        public NoteBusiness(DataService dataService) : base(dataService)
        {
        }
    }
}
