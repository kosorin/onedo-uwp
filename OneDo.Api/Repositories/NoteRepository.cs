using OneDo.Domain.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Domain.Model.Entities;
using OneDo.Domain.Model.ValueObjects;
using OneDo.Infrastructure.Data;
using OneDo.Infrastructure.Data.Entities;
using OneDo.Infrastructure.Data.Repositories;

namespace OneDo.Application.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IRepository<NoteData> noteRepository;

        public NoteRepository(IDataService dataService)
        {
            noteRepository = dataService.GetRepository<NoteData>();
        }

        public async Task<Note> Get(Guid id)
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

        public async Task Add(Note note)
        {
            var noteData = Map(note);
            await noteRepository.Add(noteData);
        }

        public async Task Update(Note note)
        {
            var noteData = Map(note);
            await noteRepository.Update(noteData);
        }

        public async Task Delete(Guid id)
        {
            await noteRepository.Delete(id);
        }


        private NoteData Map(Note note)
        {
            return new NoteData
            {
                Id = note.Id,
                FolderId = note.FolderId,
                Title = note.Title,
                Text = note.Text,
                Date = note.Date,
                Reminder = note.Reminder,
                IsFlagged = note.IsFlagged,
            };
        }

        private Note Map(NoteData noteData)
        {
            return new Note(noteData.Id, noteData.FolderId, noteData.Title, noteData.Text, noteData.Date, noteData.Reminder, noteData.IsFlagged);
        }
    }
}