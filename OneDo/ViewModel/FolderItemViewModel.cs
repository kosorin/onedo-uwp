using System;
using OneDo.Application.Queries.Folders;
using OneDo.Common.Extensions;
using Windows.UI;
using System.Threading.Tasks;
using OneDo.Application.Commands.Folders;
using OneDo.Core.Messages;
using GalaSoft.MvvmLight.Messaging;
using OneDo.Core;
using OneDo.Application;
using OneDo.Application.Models;
using OneDo.Application.Events.Folders;

namespace OneDo.ViewModel
{
    public class FolderItemViewModel : ListItemViewModel<FolderModel>
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


        public FolderItemViewModel(FolderModel model, IApi api, UIHost uiHost) : base(model.Id, api, uiHost)
        {
            Update(model);

            Api.EventBus.Subscribe<FolderUpdatedEvent>(x => Update(x.Model), x => x.Model.Id == Id);
        }

        protected override void Update(FolderModel model)
        {
            Name = model.Name;
            Color = model.Color.ToColor();
        }


        protected override void ShowEditor()
        {
            Messenger.Default.Send(new ShowFolderEditorMessage(Id));
        }

        protected override async Task Delete()
        {
            await Api.CommandBus.Execute(new DeleteFolderCommand(Id));
        }

        protected override bool CanDelete()
        {
            return AllowDelete;
        }
    }
}