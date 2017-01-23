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
    public class NoteEditorParameters : IParameters
    {
        public NoteEditorParameters(Guid? entityId, Guid selectedFolderId)
        {
            EntityId = entityId;
            SelectedFolderId = selectedFolderId;
        }

        public Guid? EntityId { get; }

        public Guid SelectedFolderId { get; set; }
    }
}