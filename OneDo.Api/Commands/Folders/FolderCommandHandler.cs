using OneDo.Application.Common;
using OneDo.Application.Queries.Folders;
using OneDo.Infrastructure.Entities;
using OneDo.Infrastructure.Repositories;
using OneDo.Infrastructure.Repositories.Domain;
using OneDo.Infrastructure.Services.DataService;
using OneDo.Domain;
using OneDo.Domain.Common;
using OneDo.Domain.Model;
using OneDo.Domain.Model.Entities;
using OneDo.Domain.Model.Repositories;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Folders
{
    public class FolderCommandHandler :
        ICommandHandler<SaveFolderCommand>,
        ICommandHandler<DeleteFolderCommand>,
        ICommandHandler<DeleteAllFoldersCommand>
    {
        private readonly IFolderRepository folderRepository;

        private readonly IQueryRepository<FolderData> folderQueryRepository;

        public FolderCommandHandler(IDataService dataService)
        {
            folderRepository = new FolderRepository(dataService);
            folderQueryRepository = dataService.RepositoryFactory.GetQueryRepository<FolderData>();
        }

        public async Task Handle(SaveFolderCommand command)
        {
            var folder = await folderRepository.Get(command.Id);
            if (folder != null)
            {
                folder.ChangeName(command.Name);
                folder.ChangeColor(new Color(command.Color));
                await folderRepository.Update(folder);
            }
            else
            {
                folder = new Folder(command.Id, command.Name, new Color(command.Color));
                await folderRepository.Add(folder);
            }
        }

        public async Task Handle(DeleteFolderCommand command)
        {
            await folderRepository.Delete(command.Id);
        }

        public async Task Handle(DeleteAllFoldersCommand args)
        {
            var folderDatas = await folderQueryRepository.GetAll();
            foreach (var folderData in folderDatas)
            {
                await folderRepository.Delete(folderData.Id);
            }
        }
    }
}
