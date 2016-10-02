using GalaSoft.MvvmLight;
using OneDo.Common.Media;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace OneDo.ViewModel.Items
{
    public class FolderItemObject : ItemObject<Folder>
    {
        public string Name => Entity.Name;

        public Color Color => Common.Media.ColorHelper.FromHex(Entity.Color);

        public IFolderCommands FolderCommands { get; }

        public FolderItemObject(Folder entity, IFolderCommands folderCommands) : base(entity)
        {
            FolderCommands = folderCommands;
        }
    }
}