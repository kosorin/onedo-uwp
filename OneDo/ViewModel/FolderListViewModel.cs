using OneDo.Application;
using OneDo.Application.Queries.Folders;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public class FolderListViewModel : ListViewModel<FolderModel, FolderItemObject>, IFolderCommands
    {
        public FolderListViewModel(Api api, UIHost uiHost)
            : base(api, uiHost)
        {
        }

        public async Task Load()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var folders = await Api.Folders.GetAll();
                var folderItems = folders.Select(x => new FolderItemObject(x, this));
                Items = new ObservableCollection<FolderItemObject>(folderItems);
                Items.CollectionChanged += (s, e) =>
                {
                    DeleteCommand.RaiseCanExecuteChanged();
                };
                SelectedItem = Items.FirstOrDefault();
            });
        }


        protected override FolderItemObject CreateItem(Folder entity)
        {
            return new FolderItemObject(entity, this);
        }

        protected override EditorViewModel<Folder> CreateEditor(FolderItemObject item)
        {
            return new FolderEditorViewModel(Api, UIHost.ProgressService, item?.EntityModel);
        }

        protected override bool CanDelete(FolderItemObject item)
        {
            return Items.Count > 1;
        }


        public async Task Move(FolderItemObject folder, NoteItemObject note)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                note.EntityModel.FolderId = folder.EntityModel.Id;
                await Api.Notes.Update(note.EntityModel);
            });
            SelectedItem = folder;
        }


        protected override void OnEntityAdded(Folder entity)
        {
            base.OnEntityAdded(entity);
            SelectedItem = GetItem(entity);
        }

        protected override void OnEntityDeleted(Folder entity)
        {
            var index = Items.IndexOf(SelectedItem);
            base.OnEntityDeleted(entity);
            if (Items.Count > index)
            {
                SelectedItem = Items.ElementAt(index);
            }
            else
            {
                SelectedItem = Items.FirstOrDefault();
            }
        }

        protected override void OnEntityBulkDeleted(List<Folder> entities)
        {
            var index = Items.IndexOf(SelectedItem);
            base.OnEntityBulkDeleted(entities);
            if (Items.Count > index)
            {
                SelectedItem = Items.ElementAt(index);
            }
            else
            {
                SelectedItem = Items.FirstOrDefault();
            }
        }
    }
}
