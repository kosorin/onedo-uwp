using OneDo.Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace OneDo.Model.Data.Entities
{
    [DebuggerDisplay("{Id}: {Title}")]
    public class Note : IEntity
    {
        public int Id { get; set; }


        public string Title { get; set; }

        public string Text { get; set; }


        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }


        public bool IsFlagged { get; set; }


        public int? FolderId { get; set; }


        public DateTime? Completed { get; set; }
    }
}
