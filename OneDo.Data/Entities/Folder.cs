using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Data.Entities
{
    [DebuggerDisplay("{Id}: {Name}")]
    [Table("Folders")]
    public class Folder : IEntity
    {
        [PrimaryKey, AutoIncrement, NotNull, Unique]
        public Guid Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public string Color { get; set; }


        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
