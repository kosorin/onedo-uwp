using OneDo.Application.Common;
using OneDo.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Notes
{
    public class SaveNoteCommand : ICommand
    {
        public SaveNoteCommand(NoteModel model)
        {
            Model = model;
        }

        public NoteModel Model { get; }
    }
}
