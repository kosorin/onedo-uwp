using GalaSoft.MvvmLight;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Items
{
    public class NoteItemViewModel : ExtendedViewModel
    {
        public bool IsCompleted => Entity.Completed != null;

        public string Title => Entity.Title;


        public Note Entity { get; }

        public NoteItemViewModel(Note entity)
        {
            Entity = entity;
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(IsCompleted));
            RaisePropertyChanged(nameof(Title));
        }
    }
}
