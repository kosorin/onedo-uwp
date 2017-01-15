using OneDo.Application.Common;
using OneDo.Data.Repositories.Domain;
using OneDo.Data.Services.DataService;
using OneDo.Domain;
using OneDo.Domain.Common;
using OneDo.Domain.Model;
using OneDo.Domain.Model.Entities;
using OneDo.Domain.Model.Repositories;
using OneDo.Domain.Model.ValueObjects;
using OneDo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Folders
{
    public class FolderCommandHandler :
        ICommandHandler<SaveFolderCommand>,
        ICommandHandler<DeleteFolderCommand>
    {
        private readonly IFolderRepository folderRepository;

        public FolderCommandHandler(IDataService dataService)
        {
            folderRepository = new FolderRepository(dataService);
        }

        public async Task Handle(SaveFolderCommand command)
        {
            var folder = await folderRepository.Get(command.Id);
            if (folder != null && !folder.IsTransient())
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
    }
}
