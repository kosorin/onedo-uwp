using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace OneDo.Application.Models
{
    public class FolderModel : IModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}
