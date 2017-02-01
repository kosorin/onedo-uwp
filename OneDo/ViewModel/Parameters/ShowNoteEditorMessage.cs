using GalaSoft.MvvmLight.Messaging;
using OneDo.Application.Common;
using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Parameters
{
    public class ShowNoteEditorMessage : GenericMessage<Guid?>
    {
        public ShowNoteEditorMessage(Guid? entityId) : base(entityId)
        {
        }
    }
}