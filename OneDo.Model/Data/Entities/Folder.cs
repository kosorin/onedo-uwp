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
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }


        public Guid? Left { get; set; }

        public Guid? Right { get; set; }


        public virtual ICollection<Todo> Todos { get; set; }
    }
}
