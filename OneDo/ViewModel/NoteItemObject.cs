using GalaSoft.MvvmLight;
using OneDo.Model.Business;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class NoteItemObject : ItemObject<Note>
    {
        public bool IsFlagged => Entity.IsFlagged;

        public string Title => Entity.Title;

        public string Text => Entity.Text;

        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public bool IsOverdue => (DateWithReminder ?? DateTime.MaxValue) < DateTime.Now;

        public DateTime? DateWithReminder => dateTimeBusiness.CombineDateAndTime(Date, Reminder);

        public DateTime? Date => Entity.Date?.Date;

        public string DateText => dateTimeBusiness.DateToShortString(Date);

        public bool HasDate => Date != null;

        public TimeSpan? Reminder => Entity.Reminder;

        public string ReminderText => dateTimeBusiness.TimeToString(Reminder);

        public bool HasReminder => Reminder != null;

        public INoteCommands Commands { get; }

        private readonly DateTimeBusiness dateTimeBusiness;

        public NoteItemObject(Note entity, DateTimeBusiness dateTimeBusiness, INoteCommands commands) : base(entity)
        {
            Commands = commands;
            this.dateTimeBusiness = dateTimeBusiness;
        }
    }
}
