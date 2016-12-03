using OneDo.Model.Data;
using OneDo.Model.Entities;

namespace OneDo.Model.Business
{
    public class FolderBusiness : EntityBusiness<Folder>
    {
        public FolderBusiness(DataService dataService) : base(dataService)
        {
        }

        public string NormalizeName(string name)
        {
            return (name ?? "").Trim();
        }
    }
}