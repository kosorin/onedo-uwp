﻿using SQLite.Net.Attributes;
using System;
using System.Diagnostics;

namespace OneDo.Model.Entities
{
    [DebuggerDisplay("{Id}: {Title}")]
    [Table("Notes")]
    public class Note : IEntity
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public Guid Id { get; set; }


        [NotNull]
        public string Title { get; set; }

        [NotNull]
        public string Text { get; set; }


        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }


        public bool IsFlagged { get; set; }


        [Indexed]
        public Guid FolderId { get; set; }


        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
