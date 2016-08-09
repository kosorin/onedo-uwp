using OneDo.Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace OneDo.Model.Data.Objects
{
    [DebuggerDisplay("{Id}: {Title}")]
    public class Todo : IModel<Todo>
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public Todo Parent { get; set; }


        public string Title { get; set; }

        public string Note { get; set; }


        public DateTime? Date { get; set; }

        public TimeSpan? Reminder { get; set; }


        public bool Flag { get; set; }


        public Guid? FolderId { get; set; }

        public virtual Folder Folder { get; set; }


        public DateTime? Updated { get; set; }

        public DateTime? Completed { get; set; }

        public DateTime? Deleted { get; set; }


        public Todo Clone() => new Todo
        {
#warning Správně klonovat!
            Id = Id,
            Parent = Parent,

            Title = Title,
            Note = Note,

            Date = Date,
            Reminder = Reminder,

            Flag = Flag,
            FolderId = FolderId,
            Folder = Folder,

            Updated = Updated,
            Completed = Completed,
            Deleted = Deleted,
        };
    }
}
