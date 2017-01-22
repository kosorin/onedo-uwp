using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneDo.Application.Queries.Notes
{
    public interface INoteQuery
    {
        Task<NoteModel> Get(Guid id);

        Task<IList<NoteModel>> GetAll();

        Task<IList<NoteModel>> GetAll(Guid folderId);
    }
}