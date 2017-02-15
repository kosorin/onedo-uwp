using OneDo.Application.Common;
using OneDo.Application.Repositories;
using OneDo.Infrastructure.Data;
using OneDo.Domain.Model.Entities;
using OneDo.Domain.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Application.Core;
using OneDo.Application.Events;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Events.Notes;
using OneDo.Application.Notifications;
using OneDo.Common.Logging;

namespace OneDo.Application.Commands
{
    public class NoteCommandHandler :
        ICommandHandler<SaveNoteCommand>,
        ICommandHandler<MoveNoteToFolderCommand>,
        ICommandHandler<SetNoteFlagCommand>,
        ICommandHandler<DeleteNoteCommand>
    {
        private readonly EventBus eventBus;

        private readonly INotificationService notificationService;

        private readonly INoteRepository noteRepository;

        public NoteCommandHandler(EventBus eventBus, INotificationService notificationService, INoteRepository noteRepository)
        {
            this.eventBus = eventBus;
            this.notificationService = notificationService;
            this.noteRepository = noteRepository;
        }

        public async Task Handle(SaveNoteCommand command)
        {
            var model = command.Model;
            if (model.Id == Guid.Empty)
            {
                throw new InvalidOperationException($"Model '{model.Title}' has empty id");
            }

            var note = await noteRepository.Get(model.Id);
            if (note != null)
            {
                note.MoveToFolder(model.FolderId);
                note.ChangeTitle(model.Title);
                note.ChangeText(model.Text);
                note.ChangeDate(model.Date);
                note.ChangeReminder(model.Reminder);
                note.SetFlag(model.IsFlagged);
                await noteRepository.Update(note);
                notificationService.Reschedule(note);
                eventBus.Publish(new NoteUpdatedEvent(model));
            }
            else
            {
                note = new Note(model.Id, model.FolderId, model.Title, model.Text, model.Date, model.Reminder, null, model.IsFlagged);
                await noteRepository.Add(note);
                notificationService.Schedule(note);
                eventBus.Publish(new NoteAddedEvent(model));
            }
        }

        public async Task Handle(MoveNoteToFolderCommand command)
        {
            var note = await noteRepository.Get(command.Id);
            if (note != null)
            {
                note.MoveToFolder(command.FolderId);
                await noteRepository.Update(note);
                eventBus.Publish(new NoteMovedToFolderEvent(note.Id, note.FolderId));
            }
        }

        public async Task Handle(SetNoteFlagCommand command)
        {
            var note = await noteRepository.Get(command.Id);
            if (note != null)
            {
                note.SetFlag(command.IsFlagged);
                await noteRepository.Update(note);
                eventBus.Publish(new NoteFlagChangedEvent(note.Id, note.IsFlagged));
            }
        }

        public async Task Handle(DeleteNoteCommand command)
        {
            var id = command.Id;
            await noteRepository.Delete(id);
            notificationService.CancelScheduled(id);
            eventBus.Publish(new NoteDeletedEvent(id));
        }
    }
}
