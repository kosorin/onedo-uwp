using OneDo.Application.Common;
using OneDo.Application.Models;
using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace OneDo.Application.Events.Notes
{
    public class NoteUpdatedEvent : IEvent
    {
        public NoteUpdatedEvent(NoteModel model)
        {
            Model = model;
        }

        public NoteModel Model { get; }
    }
}