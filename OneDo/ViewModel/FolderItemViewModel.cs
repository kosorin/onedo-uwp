using System;
using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using Windows.UI;
using System.Threading.Tasks;
using OneDo.Application.Commands.Folders;
using OneDo.Core.CommandMessages;
using GalaSoft.MvvmLight.Messaging;
using OneDo.Core;
using OneDo.Application;
using OneDo.Core.EventMessages;

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

        private bool allowDelete = true;
        public bool AllowDelete
        {
            get { return allowDelete; }
            set
            {
                if (Set(ref allowDelete, value, nameof(AllowDelete)))
                {
                    DeleteCommand.RaiseCanExecuteChanged();
                }
            }
        }


        public FolderItemViewModel(FolderModel entity, IApi api, UIHost uiHost) : base(entity.Id, api, uiHost)
        {
            Update(entity);
        }

        public override void Update(FolderModel entity)
        {
            Name = entity.Name;
            Color = entity.Color.ToColor();
        }


        protected override void ShowEditor()
        {
            Messenger.Default.Send(new ShowFolderEditorMessage(Id));
        }

        protected override async Task Delete()
        {
            await Api.CommandBus.Execute(new DeleteFolderCommand(Id));
            Messenger.Default.Send(new FolderDeletedMessage(Id));
        }

        protected override bool CanDelete()
        {
            return AllowDelete;
        }
    }
}