using OneDo.Application.Common;
using OneDo.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Folders
{
    public class SaveFolderCommand : ICommand
    {
        public SaveFolderCommand(FolderModel model)
        {
            Model = model;
        }

        public FolderModel Model { get; }
    }
}
