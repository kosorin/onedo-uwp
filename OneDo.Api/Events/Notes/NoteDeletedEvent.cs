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
    public class NoteDeletedEvent : IEvent
    {
        public NoteDeletedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}