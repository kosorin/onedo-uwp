using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneDo.Application.Queries.Notes
{
    public interface INoteQuery
    {
        Task<IList<NoteModel>> GetAll();

        Task<IList<NoteModel>> GetAll(Guid folderId);
    }
}