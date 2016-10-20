using OneDo.Model.Data;

namespace OneDo.Model.Business
{
    public abstract class DataBusinessBase : BusinessBase
    {
        public DataService DataService { get; set; }

        public DataBusinessBase(DataService dataService)
        {
            DataService = dataService;
        }
    }
}
