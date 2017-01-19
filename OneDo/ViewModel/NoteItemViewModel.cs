using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using System;

namespace OneDo.ViewModel
{
    public class NoteItemViewModel : ItemViewModel<NoteModel>
    {
        public bool IsFlagged => Entity.IsFlagged;

        public string Title => Entity.Title;

        public string Text => Entity.Text;

        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public bool IsOverdue => (DateWithReminder ?? DateTime.MaxValue) < DateTime.Now;

        public DateTime? DateWithReminder => HasReminder ? Date + Reminder : Date;

        public DateTime? Date => Entity.Date?.Date;

        public string DateText => Date?.ToShortDateString();

        public bool HasDate => Date != null;

        public TimeSpan? Reminder => Entity.Reminder;

        public string ReminderText => Reminder?.ToTimeString();

        public bool HasReminder => Reminder != null;

        public INoteCommands Commands { get; }

        public NoteItemViewModel(NoteModel entityModel, INoteCommands commands) : base(entityModel)
        {
            Commands = commands;
        }
    }
}
