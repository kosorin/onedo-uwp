using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneDo.Application.Queries
{
    public interface IFolderQuery
    {
        Task<IList<FolderDTO>> GetAll();

        bool IsNameValid(string name);
    }
}