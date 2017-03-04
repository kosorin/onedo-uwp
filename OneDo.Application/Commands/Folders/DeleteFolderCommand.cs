using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Folders
{
    public class DeleteFolderCommand : ICommand
    {
        public DeleteFolderCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}