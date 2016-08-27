using GalaSoft.MvvmLight;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Items
{
    public class NoteItemObject : ItemObject<Note>
    {
        public bool IsCompleted => Entity.Completed != null;

        public string Title => Entity.Title;


        public NoteItemObject(Note entity) : base(entity)
        {

        }
    }
}
