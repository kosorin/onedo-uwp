using OneDo.Model.Entities;
using Windows.UI;

namespace OneDo.ViewModel
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