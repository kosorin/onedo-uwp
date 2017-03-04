using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Notes
{
    public class MoveNoteToFolderCommand : ICommand
    {
        public MoveNoteToFolderCommand(Guid id, Guid folderId)
        {
            Id = id;
            FolderId = folderId;
        }

        public Guid Id { get; }

        public Guid FolderId { get; }
    }
}