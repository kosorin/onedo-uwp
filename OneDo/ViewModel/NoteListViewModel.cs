using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OneDo.Common.Mvvm;
using OneDo.Application.Queries.Notes;
using OneDo.Application;

namespace OneDo.ViewModel
{
    public class NoteListViewModel : ListViewModel<NoteModel, NoteItemObject>, INoteCommands
    {
        public IExtendedCommand ToggleFlagCommand { get; }


        public FolderListViewModel FolderList { get; }

        public DateTimeBusiness DateTimeBusiness { get; }

        public NoteListViewModel(Api api, UIHost uiHost, FolderListViewModel folderList) : base(api, uiHost)
        {
            FolderList = folderList;
            DateTimeBusiness = new DateTimeBusiness(Api);

            ToggleFlagCommand = new AsyncRelayCommand<NoteItemObject>(ToggleFlag);
        }

        public async Task Load()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var folderId = FolderList.SelectedItem?.EntityModel.Id;
                var notes = await Api.Notes.GetAll(x => x.FolderId == folderId);
                Items = new ObservableCollection<NoteItemObject>(notes.Select(CreateItem));
            });
        }


        protected override NoteItemObject CreateItem(Note entity)
        {
            return new NoteItemObject(entity, DateTimeBusiness, this);
        }

        protected override EditorViewModel<Note> CreateEditor(NoteItemObject item)
        {
            return new NoteEditorViewModel(Api, UIHost.ProgressService, FolderList, item?.EntityModel);
        }

        protected override bool CanContain(Note entity)
        {
            return entity.FolderId == FolderList.SelectedItem?.EntityModel.Id;
        }


        private async Task ToggleFlag(NoteItemObject item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                item.EntityModel.IsFlagged = !item.EntityModel.IsFlagged;
                await Api.Notes.Update(item.EntityModel);
            });
        }
    }
}
