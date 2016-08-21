using OneDo.Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;

namespace OneDo.Model.Data.Entities
{
    [DebuggerDisplay("{Id}: {Name}")]
    public class Folder : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }


        public int? Left { get; set; }

        public int? Right { get; set; }


        public int? ParentId { get; set; }

        public virtual Folder Parent { get; set; }

        public virtual ICollection<Folder> Subfolders { get; set; }


        public virtual ICollection<Todo> Todos { get; set; }
    }
}
