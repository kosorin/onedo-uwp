using GalaSoft.MvvmLight.Messaging;
using OneDo.Application.Common;
using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Core.EventMessages
{
    public class NoteDeletedMessage : MessageBase
    {
        public NoteDeletedMessage(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}