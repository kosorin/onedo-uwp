using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.Folders
{
    public interface IFolderRepository
    {
        Task Add(Folder folder);

        Task Update(Folder folder);

        Task Delete(Folder folder);

        Task DeleteAll();

        Task<IList<Folder>> GetAll();
    }
}
