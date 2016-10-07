using OneDo.Model.Data;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace OneDo.Model.Data.Entities
{
    [DebuggerDisplay("{Id}: {Title}")]
    [Table("Notes")]
    public class Note : IEntity
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public int Id { get; set; }


        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string Text { get; set; }


        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }


        [NotNull]
        public bool IsFlagged { get; set; }


        [Indexed]
        public int? FolderId { get; set; }
    }
}
