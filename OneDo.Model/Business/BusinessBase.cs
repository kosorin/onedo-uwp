using OneDo.Model.Data;

namespace OneDo.Model.Business
{
    public abstract class BusinessBase
    {
        public DataService DataService { get; set; }

        public BusinessBase(DataService dataService)
        {
            DataService = dataService;
        }
    }
}
