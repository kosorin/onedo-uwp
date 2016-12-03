using SQLite.Net.Attributes;
using System;
using System.Diagnostics;

namespace OneDo.Model.Entities
{
    [DebuggerDisplay("{Id}: {Name}")]
    [Table("Folders")]
    public class Folder : IEntity
    {
        [PrimaryKey, AutoIncrement, NotNull]
        public Guid Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Color { get; set; }


        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
