using OneDo.Application.Queries.Notes;
using OneDo.Common.Extensions;
using System;

namespace OneDo.ViewModel
{
    public class NoteItemObject : ItemObject<NoteModel>
    {
        public bool IsFlagged => EntityModel.IsFlagged;

        public string Title => EntityModel.Title;

        public string Text => EntityModel.Text;

        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public bool IsOverdue => (DateWithReminder ?? DateTime.MaxValue) < DateTime.Now;

        public DateTime? DateWithReminder => HasReminder ? Date + Reminder : Date;

        public DateTime? Date => EntityModel.Date?.Date;

        public string DateText => Date?.ToShortDateString();

        public bool HasDate => Date != null;

        public TimeSpan? Reminder => EntityModel.Reminder;

        public string ReminderText => Reminder?.ToTimeString();

        public bool HasReminder => Reminder != null;

        public INoteCommands Commands { get; }

        public NoteItemObject(NoteModel entityModel, INoteCommands commands) : base(entityModel)
        {
            Commands = commands;
        }
    }
}
