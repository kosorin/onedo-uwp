using OneDo.Application.Common;
using OneDo.Application.Models;
using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyMessenger;

namespace OneDo.Application.Events.Folders
{
    public class FolderUpdatedEvent : IEvent
    {
        public FolderUpdatedEvent(FolderModel model)
        {
            Model = model;
        }

        public FolderModel Model { get; }
    }
}