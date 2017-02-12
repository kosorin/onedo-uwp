using OneDo.Application.Common;
using OneDo.Application.Queries.Folders;
using OneDo.Infrastructure.Data.Entities;
using OneDo.Infrastructure.Data.Repositories;
using OneDo.Application.Repositories;
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
using OneDo.Infrastructure.Data;
using OneDo.Application.Core;
using OneDo.Application.Events;
using OneDo.Application.Commands.Folders;
using OneDo.Application.Events.Folders;
using OneDo.Application.Notifications;

namespace OneDo.Application.Commands
{
    public class FolderCommandHandler :
        ICommandHandler<SaveFolderCommand>,
        ICommandHandler<DeleteFolderCommand>,
        ICommandHandler<DeleteAllFoldersCommand>
    {
        private readonly EventBus eventBus;

        private readonly INotificationService notificationService;

        private readonly IFolderRepository folderRepository;

        private readonly IQueryRepository<FolderData> folderQueryRepository;

        private readonly IQueryRepository<NoteData> noteQueryRepository;

        public FolderCommandHandler(EventBus eventBus, INotificationService notificationService, IFolderRepository folderRepository, IQueryRepository<FolderData> folderQueryRepository, IQueryRepository<NoteData> noteQueryRepository)
        {
            this.eventBus = eventBus;
            this.notificationService = notificationService;
            this.folderRepository = folderRepository;
            this.folderQueryRepository = folderQueryRepository;
            this.noteQueryRepository = noteQueryRepository;
        }

        public async Task Handle(SaveFolderCommand command)
        {
            var model = command.Model;
            if (model.Id == Guid.Empty)
            {
                throw new InvalidOperationException($"Model '{model.Name}' has empty id");
            }

            var folder = await folderRepository.Get(model.Id);
            if (folder != null)
            {
                folder.ChangeName(model.Name);
                folder.ChangeColor(new Color(model.Color));
                await folderRepository.Update(folder);
                eventBus.Publish(new FolderUpdatedEvent(model));
            }
            else
            {
                folder = new Folder(model.Id, model.Name, new Color(model.Color));
                await folderRepository.Add(folder);
                eventBus.Publish(new FolderAddedEvent(model));
            }
        }

        public async Task Handle(DeleteFolderCommand command)
        {
            await DeleteFolder(command.Id);
        }

        public async Task Handle(DeleteAllFoldersCommand command)
        {
            var folderDatas = await folderQueryRepository.GetAll();
            foreach (var folderData in folderDatas)
            {
                await DeleteFolder(folderData.Id);
            }
        }

        private async Task DeleteFolder(Guid folderId)
        {
            var noteDatas = await noteQueryRepository.GetAll(x => x.FolderId == folderId);
            foreach (var noteData in noteDatas)
            {
                notificationService.CancelScheduled(noteData.Id);
            }
            await folderRepository.Delete(folderId);
            eventBus.Publish(new FolderDeletedEvent(folderId));
        }
    }
}
