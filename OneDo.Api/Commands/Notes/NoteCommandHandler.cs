﻿using OneDo.Application.Common;
using OneDo.Data.Repositories.Domain;
using OneDo.Data.Services.DataService;
using OneDo.Domain.Model.Entities;
using OneDo.Domain.Model.Repositories;
using OneDo.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Notes
{
    public class NoteCommandHandler :
        ICommandHandler<SaveNoteCommand>,
        ICommandHandler<SetNoteFlagCommand>,
        ICommandHandler<DeleteNoteCommand>
    {
        private readonly INoteRepository noteRepository;

        private readonly DateTimeService dateTimeService;

        public NoteCommandHandler(IDataService dataService, DateTimeService dateTimeService)
        {
            noteRepository = new NoteRepository(dataService);
            this.dateTimeService = dateTimeService;
        }

        public async Task Handle(SaveNoteCommand command)
        {
            var note = await noteRepository.Get(command.Id);
            if (note != null && !note.IsTransient())
            {
                note.MoveToFolder(command.FolderId);
                note.ChangeTitle(command.Title);
                note.ChangeText(command.Text);
                note.ChangeDate(command.Date);
                note.ChangeReminder(command.Reminder);
                note.SetFlag(command.IsFlagged);
                await noteRepository.Update(note);
            }
            else
            {
                note = new Note(command.Id, command.FolderId, command.Title, command.Text, command.Date, command.Reminder, command.IsFlagged);
                await noteRepository.Add(note);
            }
        }

        public async Task Handle(SetNoteFlagCommand command)
        {
            var note = await noteRepository.Get(command.Id);
            if (note != null)
            {
                note.SetFlag(command.IsFlagged);
                await noteRepository.Update(note);
            }
        }

        public async Task Handle(DeleteNoteCommand command)
        {
            await noteRepository.Delete(command.Id);
        }
    }
}