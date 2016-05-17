using System;
using Windows.UI;

namespace OneDo.Model.Data.Objects
{
    public class Tag
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Color Color { get; set; }
    }
}
