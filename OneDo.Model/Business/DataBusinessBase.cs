using OneDo.Model.Data;

namespace OneDo.Model.Business
{
    public abstract class DataBusinessBase : BusinessBase
    {
        public DataService DataService { get; }

        public DataBusinessBase(DataService dataService)
        {
            DataService = dataService;
        }
    }
}
