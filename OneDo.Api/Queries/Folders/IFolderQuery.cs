using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneDo.Application.Queries.Folders
{
    public interface IFolderQuery
    {
        Task<IList<FolderModel>> GetAll();
    }
}