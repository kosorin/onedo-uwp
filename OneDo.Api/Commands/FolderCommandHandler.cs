using OneDo.Application.Common;
using OneDo.Application.Services;
using OneDo.Application.Services.DataService;
using OneDo.Domain;
using OneDo.Domain.Model;
using OneDo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Folders
{
    public class KarelCommand : ICommand
    {
        public string Name { get; set; }
    }
    public class CtiborCommand : ICommand
    {
        public string Name { get; set; }
    }

    public class FolderCommandHandler :
        ICommandHandler<KarelCommand>,
        ICommandHandler<CtiborCommand>
    {
        private readonly IDataService dataService;

        private readonly DateTimeService dateTimeService;

        public FolderCommandHandler(IDataService dataService, DateTimeService dateTimeService)
        {
            this.dataService = dataService;
            this.dateTimeService = dateTimeService;
        }

        public Task Handle(CtiborCommand args)
        {
            return Task.CompletedTask;
        }

        public Task Handle(KarelCommand args)
        {
            return Task.CompletedTask;
        }

        //public async Task Save(FolderDTO folderDTO)
        //{
        //    if (Entity.IsTransient(folderDTO.Id))
        //    {
        //        await Add(folderDTO);
        //    }
        //    else
        //    {
        //        await Update(folderDTO);
        //    }
        //}

        //private async Task Add(FolderDTO folderDTO)
        //{
        //    var folder = Map(folderDTO);
        //    await folderRepository.Add(folder);
        //}

        //private async Task Update(FolderDTO folderDTO)
        //{
        //    var folder = await folderRepository.GetById(folderDTO.Id);
        //    if (folder != null)
        //    {
        //        folder.ChangeName(folderDTO.Name);
        //        folder.ChangeColor(new Color(folderDTO.Color));
        //        await folderRepository.Update(folder);
        //    }
        //}

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
