using OneDo.Application.Common;
using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace OneDo.Application.Events.Folders
{
    public class FolderDeletedEvent : IEvent
    {
        public FolderDeletedEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}