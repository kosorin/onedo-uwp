using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class FolderEventArgs : EventArgs
    {
        public Folder Entity { get; set; }

        public FolderEventArgs(Folder entity)
        {
            Entity = entity;
        }
    }
}