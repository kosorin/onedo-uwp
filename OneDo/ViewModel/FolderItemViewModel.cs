using System;
using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using Windows.UI;

namespace OneDo.ViewModel
{
    public class FolderItemViewModel : EntityViewModel<FolderModel>
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { Set(ref name, value); }
        }

        private Color color;
        public Color Color
        {
            get { return color; }
            set { Set(ref color, value); }
        }

        public IFolderCommands FolderCommands { get; }

        public FolderItemViewModel(FolderModel entity, IFolderCommands folderCommands) : base(entity.Id)
        {
            FolderCommands = folderCommands;

            Update(entity);
        }

        public override void Update(FolderModel entity)
        {
            Name = entity.Name;
            Color = entity.Color.ToColor();
        }
    }
}