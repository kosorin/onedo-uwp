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
        ICommandHandler<SaveFolderCommand>
    {
        private readonly IFolderRepository folderRepository;

        private readonly DateTimeService dateTimeService;

        public FolderCommandHandler(IDataService dataService, DateTimeService dateTimeService)
        {
            folderRepository = new FolderRepository(dataService);
            this.dateTimeService = dateTimeService;
        }

        public async Task Handle(SaveFolderCommand command)
        {
            var folder = await folderRepository.Get(command.Id);
            if (folder != null)
            {
                folder.ChangeName(command.Name);
                folder.ChangeColor(new Color(command.Color));
            }
            else
            {
                folder = new Folder(command.Id, command.Name, new Color(command.Color));
            }
            await folderRepository.Save(folder);
        }

        //public async Task Delete(Guid id)
        //{
        //    var folder = await folderRepository.GetById(id);
        //    if (folder != null)
        //    {
        //        await folderRepository.Delete(folder);
        //    }
        //}

        //public async Task<IList<FolderDTO>> GetAll()
        //{
        //    var folders = await folderRepository.GetAll();
        //    return folders.Select(Map).ToList();
        //}

        //public bool IsNameValid(string name)
        //{
        //    return name?.Length > 0;
        //}


        //private Folder Map(FolderDTO folderDTO)
        //{
        //    return new Folder(folderDTO.Id, folderDTO.Name, new Color(folderDTO.Color), Enumerable.Empty<Guid>());
        //}

        //private FolderDTO Map(Folder folder)
        //{
        //    return new FolderDTO
        //    {
        //        Id = folder.Id,
        //        Name = folder.Name,
        //        Color = folder.Color.Hex,
        //        BadgeNumber = folder.NoteIds.Count(note => noteBadgeSpecification.IsSatisfiedBy(note)),
        //    };
        //}
    }
}
