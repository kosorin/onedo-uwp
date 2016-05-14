using System;
using Windows.UI;

namespace OneDo.Model.Entity
{
    public class Tag
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public Color Color { get; set; }
    }
}
