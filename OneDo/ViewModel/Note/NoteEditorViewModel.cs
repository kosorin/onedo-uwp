using GalaSoft.MvvmLight.Messaging;
using OneDo.Application;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Models;
using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using OneDo.Core;
using OneDo.Core.Messages;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Core;
using OneDo.ViewModel.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Note
{
    public class NoteEditorViewModel : EditorViewModel<NoteModel>
    {
        public List<FolderItemViewModel> Folders { get; }

        private FolderItemViewModel selectedFolder;
        public FolderItemViewModel SelectedFolder
        {
            get { return selectedFolder; }
            set
            {
                if (Set(ref selectedFolder, value))
                {
                    ValidateProperty();
                    MarkProperty(() => SelectedFolder?.Id != Original.FolderId);
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
                    ValidateProperty();
                    MarkProperty(() => Title.TrimNull() != Original.Title.TrimNull());
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
                    ValidateProperty();
                    MarkProperty(() => Text.TrimNull() != Original.Text.TrimNull());
                }
            }
        }

        private DateTimeOffset? reminderDate;
        public DateTimeOffset? ReminderDate
        {
            get { return reminderDate; }
            set
            {
                if (Set(ref reminderDate, value))
                {
                    ValidateProperty();
                    MarkProperty(() => ReminderDate?.Date != Original.Reminder?.DateTime.Date);
                }
            }
        }

        private TimeSpan reminderTime;
        public TimeSpan ReminderTime
        {
            get { return reminderTime; }
            set
            {
                if (Set(ref reminderTime, value))
                {
                    ValidateProperty();
                    MarkProperty(() => ReminderTime != Original.Reminder?.DateTime.TimeOfDay);
                }
            }
        }

        private bool? isFlagged;
        public bool? IsFlagged
        {
            get { return isFlagged; }
            set
            {
                if (Set(ref isFlagged, value))
                {
                    ValidateProperty();
                    MarkProperty(() => IsFlagged != Original.IsFlagged);
                }
            }
        }

        public NoteEditorViewModel(IApi api, IProgressService progressService, FolderListViewModel folderList) : base(api, progressService)
        {
            Folders = folderList.Items.ToList();
            SelectedFolder = folderList.SelectedItem;

            Rules = new Dictionary<string, Func<bool>>
            {
                [nameof(SelectedFolder)] = () => SelectedFolder != null,
                [nameof(Title)] = () => !string.IsNullOrWhiteSpace(Title),
                [nameof(IsFlagged)] = () => IsFlagged != null,
            };
        }

        protected override async Task InitializeData()
        {
            if (Id != null)
            {
                await ProgressService.RunAsync(async () =>
                {
                    var original = await Api.NoteQuery.Get((Guid)Id);
                    if (original != null)
                    {
                        Original = original;
                    }
                });
            }
        }

        protected override void InitializeProperties()
        {
            if (Id != null)
            {
                var folder = Folders.Where(x => x.Id == Original.FolderId).FirstOrDefault();
                if (folder != null)
                {
                    SelectedFolder = folder;
                }
            }

            Title = Original.Title;
            Text = Original.Text;
            ReminderDate = Original.Reminder?.DateTime.Date;
            ReminderTime = Original.Reminder?.DateTime.TimeOfDay ?? TimeSpan.FromHours(7);
            IsFlagged = Original.IsFlagged;
        }


        protected override async Task Save()
        {
            Original.FolderId = SelectedFolder.Id;
            Original.Title = Title.TrimNull();
            Original.Text = Text.TrimNull();
            Original.Reminder = ReminderDate != null ? new ReminderModel
            {
                DateTime = (DateTime)ReminderDate?.Date + ReminderTime,
            } : null;
            Original.IsFlagged = IsFlagged ?? false;

            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new SaveNoteCommand(Original));
                Messenger.Default.Send(new CloseModalMessage());
            });
        }

        protected override async Task Delete()
        {
            await ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new DeleteNoteCommand(Original.Id));
                Messenger.Default.Send(new CloseModalMessage());
            });
        }
    }
}