using OneDo.Application.Common;
using OneDo.Common.Extensions;
using OneDo.Data.Entities;
using OneDo.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Queries
{
    public class FolderQuery : IFolderQuery
    {
        private readonly IQueryRepository<Folder> folderRepository;

        public FolderQuery(IQueryRepository<Folder> folderRepository)
        {
            this.folderRepository = folderRepository;
        }

        public async Task<IList<FolderDTO>> GetAll()
        {
            var folders = await folderRepository.GetAll();
            return folders.Select(Map).ToList();
        }

        public bool IsNameValid(string name)
        {
            return name?.Length > 0;
        }

        private FolderDTO Map(Folder folder)
        {
            return new FolderDTO
            {
                Id = folder.Id,
                Name = folder.Name,
                Color = folder.Color.ToColor(),
            };
        }
    }
}
