using GalaSoft.MvvmLight.Messaging;
using OneDo.Application.Common;
using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Core.Messages
{
    public class ShowFolderEditorMessage : MessageBase
    {
        public ShowFolderEditorMessage(Guid? id)
        {
            Id = id;
        }

        public Guid? Id { get; set; }
    }
}