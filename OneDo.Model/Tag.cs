using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace OneDo.Model
{
    public class Tag
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public Color Color { get; set; }
    }
}
