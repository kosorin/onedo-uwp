using OneDo.Application.Common;
using OneDo.Common.Extensions;
using OneDo.Data.Entities;
using OneDo.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Queries.Folders
{
    public class FolderQuery : IFolderQuery
    {
        private readonly IQueryRepository<FolderData> folderRepository;

        public FolderQuery(IQueryRepository<FolderData> folderRepository)
        {
            this.folderRepository = folderRepository;
        }

        public async Task<IList<FolderModel>> GetAll()
        {
            var folders = await folderRepository.GetAll();
            return folders.Select(Map).ToList();
        }

        private FolderModel Map(FolderData folder)
        {
            return new FolderModel
            {
                Id = folder.Id,
                Name = folder.Name,
                Color = folder.Color,
            };
        }
    }
}
