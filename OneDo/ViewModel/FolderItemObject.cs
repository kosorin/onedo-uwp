using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using Windows.UI;

namespace OneDo.ViewModel
{
    public class FolderItemObject : ItemObject<FolderModel>
    {
        public string Name => EntityModel.Name;

        public Color Color => EntityModel.Color.ToColor();

        public IFolderCommands FolderCommands { get; }

        public FolderItemObject(FolderModel entityModel, IFolderCommands folderCommands) : base(entityModel)
        {
            FolderCommands = folderCommands;
        }
    }
}