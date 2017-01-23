using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using Windows.UI;

namespace OneDo.ViewModel.Items
{
    public class FolderItemViewModel : ItemViewModel<FolderModel>
    {
        public string Name { get; }

        public Color Color { get; }

        public IFolderCommands FolderCommands { get; }

        public FolderItemViewModel(FolderModel entity, IFolderCommands folderCommands) : base(entity.Id)
        {
            Name = entity.Name;
            Color = entity.Color.ToColor();

            FolderCommands = folderCommands;
        }
    }
}