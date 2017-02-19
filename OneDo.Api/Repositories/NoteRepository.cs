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
                return noteData.ToEntity();
            }
            else
            {
                return null;
            }
        }

        public async Task Add(Note note)
        {
            var noteData = NoteData.FromEntity(note);
            await noteRepository.Add(noteData);
        }

        public async Task Update(Note note)
        {
            var noteData = NoteData.FromEntity(note);
            await noteRepository.Update(noteData);
        }

        public async Task Delete(Guid id)
        {
            await noteRepository.Delete(id);
        }
    }
}