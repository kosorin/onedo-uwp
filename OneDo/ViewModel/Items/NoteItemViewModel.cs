using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using System;

namespace OneDo.ViewModel.Items
{
    public class NoteItemViewModel : ItemViewModel<NoteModel>
    {
        public Guid FolderId { get; }

        public bool IsFlagged { get; }

        public string Title { get; }

        public string Text { get; }

        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public bool IsOverdue => (DateWithReminder ?? DateTime.MaxValue) < DateTime.Now;

        public DateTime? DateWithReminder => HasReminder ? Date + Reminder : Date;

        public DateTime? Date { get; }

        public string DateText => Date?.ToShortDateString();

        public bool HasDate => Date != null;

        public TimeSpan? Reminder { get; }

        public string ReminderText => Reminder?.ToTimeString();

        public bool HasReminder => Reminder != null;

        public INoteCommands Commands { get; }

        public NoteItemViewModel(NoteModel entity, INoteCommands commands) : base(entity.Id)
        {
            FolderId = entity.FolderId;
            IsFlagged = entity.IsFlagged;
            Title = entity.Title;
            Text = entity.Text;
            Date = entity.Date?.Date;
            Reminder = entity.Reminder;

            Commands = commands;
        }
    }
}
