﻿using OneDo.Application;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Queries.Notes;
using OneDo.Services.ProgressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class NoteEditorViewModel : EditorViewModel<NoteModel>
    {
        public List<FolderItemObject> Folders { get; }

        private FolderItemObject selectedFolder;
        public FolderItemObject SelectedFolder
        {
            get { return selectedFolder; }
            set
            {
                if (Set(ref selectedFolder, value))
                {
                    UpdateDirtyProperty(() => SelectedFolder?.EntityModel.Id != Original.FolderId);
                }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (Set(ref title, value))
                {
                    UpdateDirtyProperty(() => Title != Original.Title);
                    ValidateProperty(() => !string.IsNullOrWhiteSpace(Title));
                }
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (Set(ref text, value))
                {
                    UpdateDirtyProperty(() => Text != Original.Text);
                }
            }
        }

        public DatePickerViewModel DatePicker { get; }

        public TimePickerViewModel ReminderPicker { get; }

        private bool? isFlagged;
        public bool? IsFlagged
        {
            get { return isFlagged; }
            set
            {
                if (Set(ref isFlagged, value))
                {
                    UpdateDirtyProperty(() => IsFlagged != Original.IsFlagged);
                }
            }
        }


        public Api Api { get; }

        public NoteEditorViewModel(Api api, IProgressService progressService, FolderListViewModel folderList)
            : this(api, progressService, folderList, null)
        {

        }

        public NoteEditorViewModel(Api api, IProgressService progressService, FolderListViewModel folderList, NoteModel noteModel)
            : base(progressService)
        {
            Api = api;
            IsNew = noteModel == null;
            Original = noteModel ?? CreateDefault();

            Folders = folderList.Items.ToList();
            SelectedFolder = folderList.SelectedItem;

            DatePicker = new DatePickerViewModel("Set Date & Reminder");
            DatePicker.DateChanged += (s, e) =>
            {
                UpdateDirtyProperty(() => e.Date?.Date != Original.Date?.Date);
            };
            ReminderPicker = new TimePickerViewModel("Set Reminder");
            ReminderPicker.TimeChanged += (s, e) =>
            {
                UpdateDirtyProperty(() => e.Time != Original.Reminder);
            };

            Load();
        }


        private void Load()
        {

            if (!IsNew)
            {
                var folder = Folders.Where(x => x.EntityModel.Id == Original.FolderId).FirstOrDefault();
                if (folder != null)
                {
                    SelectedFolder = folder;
                }
            }
            Title = Original.Title;
            Text = Original.Text;
            DatePicker.Date = Original.Date;
            ReminderPicker.Time = Original.Reminder;
            IsFlagged = Original.IsFlagged;
        }

        protected override async Task Save()
        {
            Original.FolderId = SelectedFolder.EntityModel.Id;
            Original.Title = Title ?? "";
            Original.Text = Text ?? "";
            Original.Date = DatePicker.Date;
            Original.Reminder = ReminderPicker.Time;
            Original.IsFlagged = IsFlagged ?? false;

            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new SaveNoteCommand(Original.Id, Original.FolderId, Original.Title, Original.Text, Original.Date, Original.Reminder, Original.IsFlagged));
            });
            OnSaved();
        }

        protected override async Task Delete()
        {
            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new DeleteNoteCommand(Original.Id));
            });
            OnDeleted();
        }
    }
}