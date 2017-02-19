using OneDo.Domain.Model.Entities;
using SQLite.Net.Attributes;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using RecurrenceVO = OneDo.Domain.Model.ValueObjects.Recurrence;

namespace OneDo.Infrastructure.Data.Entities
{
    [DebuggerDisplay("{Id}: {Title}")]
    [Table("Notes")]
    public class NoteData : IEntityData
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public Guid Id { get; set; }

        [Indexed, NotNull]
        public Guid FolderId { get; set; }


        [NotNull]
        public string Title { get; set; }

        public string Text { get; set; }


        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }

        public string Recurrence { get; set; }


        public bool IsFlagged { get; set; }


        public Note ToEntity()
        {
            return new Note(Id, FolderId, Title, Text, Date, Reminder, RecurrenceVO.Load(Recurrence), IsFlagged);
        }

        public static NoteData FromEntity(Note note)
        {
            return new NoteData
            {
                Id = note.Id,
                FolderId = note.FolderId,
                Title = note.Title,
                Text = note.Text,
                Date = note.Date,
                Reminder = note.Reminder,
                Recurrence = note.Recurrence?.Save(),
                IsFlagged = note.IsFlagged,
            };
        }
    }
}
