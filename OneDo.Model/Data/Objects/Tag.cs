using OneDo.Model.Data;
using System;
using Windows.UI;

namespace OneDo.Model.Data.Objects
{
    public class Tag : IModel<Tag>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Color Color { get; set; }


#warning Správně klonovat!
        public Tag Clone() => new Tag
        {
            Id = Id,
            Name = Name,
            Color = Color,
        };
    }
}
