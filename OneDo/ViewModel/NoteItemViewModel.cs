using GalaSoft.MvvmLight.Messaging;
using OneDo.Application;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Events.Notes;
using OneDo.Application.Models;
using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using OneDo.Core;
using OneDo.Core.Messages;
using System;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class NoteItemViewModel : ListItemViewModel<NoteModel>
    {
        public Guid FolderId { get; }

        private bool isFlagged;
        public bool IsFlagged
        {
            get { return isFlagged; }
            private set { Set(ref isFlagged, value); }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { Set(ref title, value); }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (Set(ref text, value))
                {
                    RaisePropertyChanged(nameof(HasText));
                }
            }
        }

        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public bool IsOverdue => (DateWithReminder ?? DateTime.MaxValue) < DateTime.Now;

        public DateTime? DateWithReminder => HasReminder ? Date + Reminder : Date;

        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set
            {
                if (Set(ref date, value))
                {
                    RaisePropertyChanged(nameof(IsOverdue));
                    RaisePropertyChanged(nameof(DateWithReminder));
                    RaisePropertyChanged(nameof(DateText));
                    RaisePropertyChanged(nameof(HasDate));
                }
            }
        }

        public string DateText => Date?.ToShortDateString();

        public bool HasDate => Date != null;

        private TimeSpan? reminder;
        public TimeSpan? Reminder
        {
            get { return reminder; }
            set
            {
                if (Set(ref reminder, value))
                {
                    RaisePropertyChanged(nameof(IsOverdue));
                    RaisePropertyChanged(nameof(DateWithReminder));
                    RaisePropertyChanged(nameof(ReminderText));
                    RaisePropertyChanged(nameof(HasReminder));
                }
            }
        }

        public string ReminderText => Reminder?.ToTimeString();

        public bool HasReminder => Reminder != null;


        public IExtendedCommand ToggleFlagCommand { get; }

        public IExtendedCommand MoveToFolderCommand { get; }


        public NoteItemViewModel(NoteModel model, IApi api, UIHost uiHost) : base(model.Id, api, uiHost)
        {
            FolderId = model.FolderId;
            ToggleFlagCommand = new AsyncRelayCommand(ToggleFlag);
            MoveToFolderCommand = new AsyncRelayCommand<Guid>(MoveToFolder);

            Update(model);

            Api.EventBus.Subscribe<NoteUpdatedEvent>(x => Update(x.Model), x => x.Model.Id == Id);
            Api.EventBus.Subscribe<NoteFlagChangedEvent>(x => IsFlagged = x.IsFlagged, x => x.Id == Id);
        }

        protected override void Update(NoteModel model)
        {
            IsFlagged = model.IsFlagged;
            Title = model.Title;
            Text = model.Text;
            //Date = model.Date?.Date;
            //Reminder = model.Reminder;
        }


        protected override void ShowEditor()
        {
            Messenger.Default.Send(new ShowNoteEditorMessage(Id));
        }

        protected override async Task Delete()
        {
            await Api.CommandBus.Execute(new DeleteNoteCommand(Id));
        }

        protected override bool CanDelete()
        {
            return true;
        }

        private async Task ToggleFlag()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new SetNoteFlagCommand(Id, !IsFlagged));
            });
        }

        private async Task MoveToFolder(Guid folderId)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new MoveNoteToFolderCommand(Id, folderId));
            });
        }
    }
}
