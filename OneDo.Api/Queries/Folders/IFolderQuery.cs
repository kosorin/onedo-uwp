using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneDo.Application.Queries.Folders
{
    public interface IFolderQuery
    {
        Task<FolderModel> Get(Guid id);

        Task<IList<FolderModel>> GetAll();
    }
}