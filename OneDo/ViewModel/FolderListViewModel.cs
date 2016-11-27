using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OneDo.ViewModel.Commands;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using Windows.Foundation;
using OneDo.Common.Event;
using OneDo.Common.UI;
using OneDo.Model.Business;

namespace OneDo.ViewModel
{
    public class FolderListViewModel : ListViewModel<Folder, FolderItemObject>, IFolderCommands
    {
        public FolderListViewModel(DataService dataService, UIHost uiHost)
            : base(dataService, uiHost)
        {
        }

        public async Task Load()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var folders = await DataService.Folders.GetAll();
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
            return new FolderEditorViewModel(DataService, UIHost.ProgressService, item?.Entity);
        }

        protected override bool CanDelete(FolderItemObject item)
        {
            return Items.Count > 1;
        }


        public async Task Move(FolderItemObject folder, NoteItemObject note)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                note.Entity.FolderId = folder.Entity.Id;
                await DataService.Notes.Update(note.Entity);
            });
            SelectedItem = folder;
        }


        protected override void OnEntityAdded(Folder entity)
        {
            base.OnEntityAdded(entity);
            SelectedItem = Items.Last();
        }

        protected override void OnEntityDeleted(Folder entity)
        {
            base.OnEntityDeleted(entity);
            SelectedItem = Items.FirstOrDefault();
        }
    }
}
