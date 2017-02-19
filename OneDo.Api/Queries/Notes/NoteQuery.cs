using OneDo.Application.Models;
using OneDo.Domain.Model.ValueObjects;
using OneDo.Infrastructure.Data.Entities;
using OneDo.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Queries.Notes
{
    public class NoteQuery : INoteQuery
    {
        private readonly IQueryRepository<NoteData> noteRepository;

        public NoteQuery(IQueryRepository<NoteData> noteRepository)
        {
            this.noteRepository = noteRepository;
        }

        public async Task<NoteModel> Get(Guid id)
        {
            var noteData = await noteRepository.Get(id);
            if (noteData != null)
            {
                return NoteModel.FromData(noteData);
            }
            else
            {
                return null;
            }
        }

        public async Task<IList<NoteModel>> GetAll()
        {
            var noteDatas = await noteRepository.GetAll();
            return noteDatas.Select(NoteModel.FromData).ToList();
        }

        public async Task<IList<NoteModel>> GetAll(Guid folderId)
        {
            var noteDatas = await noteRepository.GetAll(x => x.FolderId == folderId);
            return noteDatas.Select(NoteModel.FromData).ToList();
        }
    }
}
