using OneDo.Domain.Model.Entities;
using SQLite.Net.Attributes;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using ReminderVO = OneDo.Domain.Model.ValueObjects.Reminder;

namespace OneDo.Infrastructure.Data.Entities
{
    [DebuggerDisplay("{Id}: {Title}")]
    [Table("Notes")]
    public class NoteData : IEntityData
    {
        [PrimaryKey, AutoIncrement]
        public Guid Id { get; set; }

        [Indexed, NotNull]
        public Guid FolderId { get; set; }


        [NotNull]
        public string Title { get; set; }

        public string Text { get; set; }


        public string Reminder { get; set; }


        public bool IsFlagged { get; set; }


        public Note ToEntity()
        {
            return new Note(Id, FolderId, Title, Text, ReminderVO.Load(Reminder), IsFlagged);
        }

        public static NoteData FromEntity(Note note)
        {
            return new NoteData
            {
                Id = note.Id,
                FolderId = note.FolderId,
                Title = note.Title,
                Text = note.Text,
                Reminder = note.Reminder?.Save(),
                IsFlagged = note.IsFlagged,
            };
        }
    }
}
