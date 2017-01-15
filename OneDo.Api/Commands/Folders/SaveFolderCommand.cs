using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Folders
{
    public class SaveFolderCommand : ICommand
    {
        public SaveFolderCommand(Guid id, string name, string color)
        {
            Id = id;
            Name = name;
            Color = color;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }
    }
}
