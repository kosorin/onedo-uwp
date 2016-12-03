using OneDo.Model.Data;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;

namespace OneDo.Model.Data.Entities
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
