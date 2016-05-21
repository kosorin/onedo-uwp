using OneDo.Model.Data;

namespace OneDo.Model.Business
{
    public abstract class BusinessBase
    {
        public IDataProvider DataProvider { get; set; }

        public BusinessBase(IDataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }
    }
}
