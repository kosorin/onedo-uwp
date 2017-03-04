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
    public class NoteFlagChangedEvent : IEvent
    {
        public NoteFlagChangedEvent(Guid id, bool isFlagged)
        {
            Id = id;
            IsFlagged = isFlagged;
        }

        public Guid Id { get; }

        public bool IsFlagged { get; }
    }
}