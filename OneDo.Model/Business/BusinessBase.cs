using OneDo.Model.Data;

namespace OneDo.Model.Business
{
    public abstract class BusinessBase
    {
        public IDataService DataService { get; set; }

        public BusinessBase(IDataService dataService)
        {
            DataService = dataService;
        }
    }
}
