using OneDo.Application;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneDo.ViewModel
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

        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set
            {
                if (Set(ref date, value))
                {
                    ValidateProperty();
                    MarkProperty(() => Date?.Date != Original.Date?.Date);
                    RaisePropertyChanged(nameof(DateText));
                }
            }
        }

        public string DateText => Date?.ToLongDateString() ?? "Set Date & Reminder";

        private TimeSpan? reminder;
        public TimeSpan? Reminder
        {
            get { return reminder; }
            set
            {
                if (Set(ref reminder, value))
                {
                    ValidateProperty();
                    MarkProperty(() => Reminder != Original.Reminder);
                    RaisePropertyChanged(nameof(ReminderText));
                }
            }
        }

        public string ReminderText => Reminder?.ToTimeString() ?? "Set Reminder";

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

        public IExtendedCommand ClearDateCommand { get; }

        public IExtendedCommand ClearReminderCommand { get; }

        public NoteEditorViewModel(Api api, IProgressService progressService, FolderListViewModel folderList) : base(api, progressService)
        {
            Folders = folderList.Items.ToList();
            SelectedFolder = folderList.SelectedItem;

            ClearDateCommand = new RelayCommand(() => Date = null);
            ClearReminderCommand = new RelayCommand(() => Reminder = null);

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
            Date = Original.Date;
            Reminder = Original.Reminder;
            IsFlagged = Original.IsFlagged;
        }


        protected override async Task Save()
        {
            Original.FolderId = SelectedFolder.Id;
            Original.Title = Title.TrimNull();
            Original.Text = Text.TrimNull();
            Original.Date = Date;
            Original.Reminder = Reminder;
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