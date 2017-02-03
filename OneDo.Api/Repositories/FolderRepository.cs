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
    public class FolderRepository : IFolderRepository
    {
        private readonly IRepository<FolderData> folderRepository;

        private readonly IRepository<NoteData> noteRepository;

        public FolderRepository(IDataService dataService)
        {
            folderRepository = dataService.GetRepository<FolderData>();
            noteRepository = dataService.GetRepository<NoteData>();
        }

        public async Task<Folder> Get(Guid id)
        {
            var folderData = await folderRepository.Get(id);
            if (folderData != null)
            {
                return Map(folderData);
            }
            else
            {
                return null;
            }
        }

        public async Task Add(Folder folder)
        {
            var folderData = Map(folder);
            await folderRepository.Add(folderData);
        }

        public async Task Update(Folder folder)
        {
            var folderData = Map(folder);
            await folderRepository.Update(folderData);
        }

        public async Task Delete(Guid id)
        {
            await noteRepository.DeleteAll(x => x.FolderId == id);
            await folderRepository.Delete(id);
        }


        private FolderData Map(Folder folder)
        {
            return new FolderData
            {
                Id = folder.Id,
                Name = folder.Name,
                Color = folder.Color.Hex,
            };
        }

        private Folder Map(FolderData folderData)
        {
            return new Folder(folderData.Id, folderData.Name, new Color(folderData.Color));
        }
    }
}
