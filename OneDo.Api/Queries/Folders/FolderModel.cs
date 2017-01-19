using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace OneDo.Application.Queries.Folders
{
    public class FolderModel : IEntityModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}
