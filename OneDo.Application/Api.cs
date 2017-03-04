using OneDo.Application.Commands.Folders;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Core;
using OneDo.Application.Models;
using OneDo.Application.Queries;
using OneDo.Application.Queries.Folders;
using OneDo.Application.Queries.Notes;
using OneDo.Common;
using OneDo.Infrastructure.Data;
using OneDo.Infrastructure.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace OneDo.Application
{
    public class Api : IApi, IDisposable
    {
        public EventBus EventBus { get; }

        public CommandBus CommandBus { get; }

        public IFolderQuery FolderQuery { get; }

        public INoteQuery NoteQuery { get; }

        private readonly DataService dataService;

        public Api()
        {
            dataService = new DataService();

            EventBus = new EventBus();
            CommandBus = new CommandBus(EventBus, dataService);

            FolderQuery = new FolderQuery(dataService.GetQueryRepository<FolderData>());
            NoteQuery = new NoteQuery(dataService.GetQueryRepository<NoteData>());
        }

        public async Task SavePreviewData()
        {
            await CommandBus.Execute(new SaveFolderCommand(new FolderModel { Id = Guid.NewGuid(), Name = "Inbox", Color = "#0063AF" }));
            await CommandBus.Execute(new SaveFolderCommand(new FolderModel { Id = Guid.NewGuid(), Name = "Work", Color = "#0F893E" }));
            await CommandBus.Execute(new SaveFolderCommand(new FolderModel { Id = Guid.NewGuid(), Name = "Shopping list", Color = "#AC008C" }));
            await CommandBus.Execute(new SaveFolderCommand(new FolderModel { Id = Guid.NewGuid(), Name = "Vacation", Color = "#F7630D" }));
            var folders = await FolderQuery.GetAll();

            var folder = folders.FirstOrDefault();
            var folder2 = folders.Skip(1).FirstOrDefault();
            await CommandBus.Execute(new SaveNoteCommand(new NoteModel { Id = Guid.NewGuid(), FolderId = folder.Id, Title = "Buy milk" }));
            await CommandBus.Execute(new SaveNoteCommand(new NoteModel
            {
                Id = Guid.NewGuid(),
                FolderId = folder.Id,
                Title = "Walk Max with bike",
                Reminder = new ReminderModel
                {
                    DateTime = DateTime.Today + TimeSpan.FromHours(7.25),
                    Recurrence = new DaysOfWeekRecurrenceModel
                    {
                        Every = 2,
                        DaysOfWeek = DaysOfWeek.Weekends
                    }
                }
            }));
            await CommandBus.Execute(new SaveNoteCommand(new NoteModel { Id = Guid.NewGuid(), FolderId = folder.Id, Title = "Call mom", Reminder = new ReminderModel { DateTime = DateTime.Today + TimeSpan.FromHours(15) }, IsFlagged = true }));
            await CommandBus.Execute(new SaveNoteCommand(new NoteModel
            {
                Id = Guid.NewGuid(),
                FolderId = folder.Id,
                Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
                Text = "Proin et diam at lorem egestas ullamcorper. Curabitur non eleifend mi. Praesent eu sem elementum, rutrum neque id, sollicitudin dolor. Proin molestie ullamcorper sem a hendrerit. Integer ac sapien erat. Morbi vehicula venenatis dolor, non aliquet nibh mattis sed.",
            }));
            await CommandBus.Execute(new SaveNoteCommand(new NoteModel { Id = Guid.NewGuid(), FolderId = folder2.Id, Title = "Test note", IsFlagged = true }));
        }


        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dataService?.Dispose();
                }

                disposed = true;
            }
        }
    }
}
