using GalaSoft.MvvmLight;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModels.Items
{
    public class FolderItemViewModel : ExtendedViewModel
    {
        public string Name => Folder.Name;

        public string Color => Folder.Color;


        public Folder Folder { get; }

        public FolderItemViewModel(Folder folder)
        {
            Folder = folder;
        }

        public void Refresh()
        {
            RaisePropertyChanged(nameof(Name));
            RaisePropertyChanged(nameof(Color));
        }
    }
}