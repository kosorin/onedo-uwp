using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class NoteEventArgs : EventArgs
    {
        public Note Entity { get; set; }

        public NoteEventArgs(Note entity)
        {
            Entity = entity;
        }
    }
}
