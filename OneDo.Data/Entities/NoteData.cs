using SQLite.Net.Attributes;
using System;
using System.Diagnostics;

namespace OneDo.Infrastructure.Entities
{
    [DebuggerDisplay("{Id}: {Title}")]
    [Table("Notes")]
    public class NoteData : IEntity
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public Guid Id { get; set; }

        [Indexed, NotNull]
        public Guid FolderId { get; set; }


        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string Text { get; set; }


        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }


        public bool IsFlagged { get; set; }
    }
}
