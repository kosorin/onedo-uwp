using OneDo.Model.Data;
using OneDo.Model.Entities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OneDo.Mvvm;
using OneDo.Model.Business;

namespace OneDo.ViewModel
{
    public class NoteListViewModel : ListViewModel<Note, NoteItemObject>, INoteCommands
    {
        public IExtendedCommand ToggleFlagCommand { get; }


        public FolderListViewModel FolderList { get; }

        public DateTimeBusiness DateTimeBusiness { get; }

        public NoteListViewModel(DataService dataService, UIHost uiHost, FolderListViewModel folderList) : base(dataService, uiHost)
        {
            FolderList = folderList;
            DateTimeBusiness = new DateTimeBusiness(DataService);

            ToggleFlagCommand = new AsyncRelayCommand<NoteItemObject>(ToggleFlag);
        }

        public async Task Load()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var folderId = FolderList.SelectedItem?.Entity.Id;
                var notes = await DataService.Notes.GetAll(x => x.FolderId == folderId);
                Items = new ObservableCollection<NoteItemObject>(notes.Select(CreateItem));
            });
        }


        protected override NoteItemObject CreateItem(Note entity)
        {
            return new NoteItemObject(entity, DateTimeBusiness, this);
        }

        protected override EditorViewModel<Note> CreateEditor(NoteItemObject item)
        {
            return new NoteEditorViewModel(DataService, UIHost.ProgressService, FolderList, item?.Entity);
        }

        protected override bool CanContain(Note entity)
        {
            return entity.FolderId == FolderList.SelectedItem?.Entity.Id;
        }


        private async Task ToggleFlag(NoteItemObject item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                item.Entity.IsFlagged = !item.Entity.IsFlagged;
                await DataService.Notes.Update(item.Entity);
            });
        }
    }
}
