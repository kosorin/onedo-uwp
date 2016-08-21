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
    }
}
