using OneDo.Model.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;

namespace OneDo.Model.Data.Objects
{
    [DebuggerDisplay("{Id}: {Name}")]
    public class Folder : IModel<Folder>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }


        public virtual ICollection<Todo> Todos { get; set; }


        public Folder Clone() => new Folder
        {
            Id = Id,
            Name = Name,
            Color = Color,

            Todos = Todos,
        };
    }
}
