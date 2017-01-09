using OneDo.Domain.Common;
using OneDo.Domain.Model.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.Entities
{
    public class Folder : Entity
    {
        public string Name { get; private set; }

        public Color Color { get; private set; }

        public IEnumerable<Guid> NoteIds { get; private set; }

        public Folder(Guid id, string name, Color color, IEnumerable<Guid> notes) : base(id)
        {
            Name = name;
            Color = color;
            NoteIds = notes.ToList();
        }

        public void ChangeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException($"{nameof(name)} should not be null, empty or white space");
            }
            Name = name;
        }

        public void ChangeColor(Color color)
        {
            if (color == null)
            {
                throw new InvalidOperationException($"{nameof(color)} should not be null");
            }
            Color = color;
        }
    }
}
