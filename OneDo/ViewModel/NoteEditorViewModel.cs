using GalaSoft.MvvmLight.Messaging;
using OneDo.Application;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Models;
using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using OneDo.Core;
using OneDo.Core.Messages;
using OneDo.Services.ProgressService;
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

                    ClearReminderCommand.RaiseCanExecuteChanged();
                    NextDayCommand.RaiseCanExecuteChanged();
                    PreviousDayCommand.RaiseCanExecuteChanged();
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

        public List<ReminderDateType> ReminderDateTypes { get; }

        private ReminderDateType selectedReminderDateType;
        public ReminderDateType SelectedReminderDateType
        {
            get { return selectedReminderDateType; }
            set
            {
                if (Set(ref selectedReminderDateType, value))
                {
                    SetReminder(value);
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

        public IExtendedCommand ClearReminderCommand { get; }

        public IExtendedCommand NextDayCommand { get; }

        public IExtendedCommand PreviousDayCommand { get; }

        public NoteEditorViewModel(IApi api, IProgressService progressService, FolderListViewModel folderList) : base(api, progressService)
        {
            Folders = folderList.Items.ToList();
            SelectedFolder = folderList.SelectedItem;

            ReminderDateTypes = Enum.GetValues(typeof(ReminderDateType)).Cast<ReminderDateType>().ToList();

            ClearReminderCommand = new RelayCommand(() => ReminderDate = null, () => ReminderDate != null);
            NextDayCommand = new RelayCommand(() => ReminderDate = ReminderDate?.AddDays(1), () => ReminderDate != null);
            PreviousDayCommand = new RelayCommand(() => ReminderDate = ReminderDate?.AddDays(-1), () => ReminderDate != null);

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


        private void SetReminder(ReminderDateType type)
        {
            throw new NotImplementedException();
        }
    }
}