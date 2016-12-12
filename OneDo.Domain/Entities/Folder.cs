using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Entities
{
    public class Folder : Entity
    {
        public string Name { get; set; }

        public string Color { get; set; }
    }
}
