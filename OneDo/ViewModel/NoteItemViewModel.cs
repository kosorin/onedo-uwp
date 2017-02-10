using GalaSoft.MvvmLight.Messaging;
using OneDo.Application;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;
using OneDo.Core;
using OneDo.Core.CommandMessages;
using OneDo.Core.EventMessages;
using System;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class NoteItemViewModel : EntityViewModel<NoteModel>
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


        public NoteItemViewModel(NoteModel entity, IApi api, UIHost uiHost) : base(entity.Id, api, uiHost)
        {
            FolderId = entity.FolderId;
            ToggleFlagCommand = new AsyncRelayCommand(ToggleFlag);

            Update(entity);
        }

        public override void Update(NoteModel entity)
        {
            IsFlagged = entity.IsFlagged;
            Title = entity.Title;
            Text = entity.Text;
            Date = entity.Date?.Date;
            Reminder = entity.Reminder;
        }


        protected override void ShowEditor()
        {
            Messenger.Default.Send(new ShowNoteEditorMessage(Id));
        }

        protected override async Task Delete()
        {
            await Api.CommandBus.Execute(new DeleteNoteCommand(Id));
            Messenger.Default.Send(new NoteDeletedMessage(Id));
        }

        protected override bool CanDelete()
        {
            return true;
        }

        private async Task ToggleFlag()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                IsFlagged = !IsFlagged;
                await Api.CommandBus.Execute(new SetNoteFlagCommand(Id, IsFlagged));
            });
        }
    }
}
