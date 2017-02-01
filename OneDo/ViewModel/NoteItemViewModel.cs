using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using System;

namespace OneDo.ViewModel
{
    public class NoteItemViewModel : EntityViewModel<NoteModel>
    {
        public Guid FolderId { get; }

        private bool isFlagged;
        public bool IsFlagged
        {
            get { return isFlagged; }
            set { Set(ref isFlagged, value); }
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

        public INoteCommands Commands { get; }

        public NoteItemViewModel(NoteModel entity, INoteCommands commands) : base(entity.Id)
        {
            FolderId = entity.FolderId;
            Commands = commands;

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
    }
}
