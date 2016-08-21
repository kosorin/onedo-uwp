using OneDo.Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace OneDo.Model.Data.Entities
{
    [DebuggerDisplay("{Id}: {Title}")]
    public class Todo : IEntity
    {
        public int Id { get; set; }


        public string Title { get; set; }

        public string Note { get; set; }


        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }


        public bool Flag { get; set; }


        public int? FolderId { get; set; }


        public DateTime? Completed { get; set; }
    }
}
