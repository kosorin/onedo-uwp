using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using Windows.UI;

namespace OneDo.ViewModel
{
    public class FolderItemViewModel: ItemViewModel<FolderModel>
    {
        public string Name => Entity.Name;

        public Color Color => Entity.Color.ToColor();

        public IFolderCommands FolderCommands { get; }

        public FolderItemViewModel(FolderModel entityModel, IFolderCommands folderCommands) : base(entityModel)
        {
            FolderCommands = folderCommands;
        }
    }
}