using OneDo.Application.Models;
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
                return Map(noteData);
            }
            else
            {
                return null;
            }
        }

        public async Task<IList<NoteModel>> GetAll()
        {
            var noteDatas = await noteRepository.GetAll();
            return noteDatas.Select(Map).ToList();
        }

        public async Task<IList<NoteModel>> GetAll(Guid folderId)
        {
            var noteDatas = await noteRepository.GetAll(x => x.FolderId == folderId);
            return noteDatas.Select(Map).ToList();
        }


        private NoteModel Map(NoteData noteData)
        {
            return new NoteModel
            {
                Id = noteData.Id,
                FolderId = noteData.FolderId,
                Title = noteData.Title,
                Text = noteData.Text,
                Date = noteData.Date,
                Reminder = noteData.Reminder,
                IsFlagged = noteData.IsFlagged,
            };
        }
    }
}
