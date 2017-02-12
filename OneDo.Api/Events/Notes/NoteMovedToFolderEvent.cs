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
    public class NoteMovedToFolderEvent : IEvent
    {
        public NoteMovedToFolderEvent(Guid id, Guid folderId)
        {
            Id = id;
            FolderId = folderId;
        }

        public Guid Id { get; }

        public Guid FolderId { get; }
    }
}