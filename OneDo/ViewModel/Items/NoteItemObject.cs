using GalaSoft.MvvmLight;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Items
{
    public class NoteItemObject : ItemObject<Note>
    {
        public bool IsFlagged => Entity.IsFlagged;

        public string Title => Entity.Title;

        public string Text => Entity.Text;

        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public bool IsOverdue => (DateWithReminder ?? DateTime.MaxValue) < DateTime.Now;

        public DateTime? DateWithReminder => Reminder != null ? Date + Reminder : Date;

        public DateTime? Date => Entity.Date?.Date;

        public bool HasDate => Date != null;

        public TimeSpan? Reminder => Entity.Reminder;

        public bool HasReminder => Reminder > TimeSpan.Zero;

        public INoteCommands Commands { get; }

        public NoteItemObject(Note entity, INoteCommands commands) : base(entity)
        {
            Commands = commands;
        }
    }
}
