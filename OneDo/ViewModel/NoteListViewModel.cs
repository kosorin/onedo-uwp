using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OneDo.Common.Mvvm;
using OneDo.Application.Queries.Notes;
using OneDo.Application;
using System;
using OneDo.Application.Commands.Notes;

namespace OneDo.ViewModel
{
    public class NoteListViewModel : ListViewModel<NoteItemViewModel, NoteModel>, INoteCommands
    {
        public IExtendedCommand ToggleFlagCommand { get; }


        public FolderListViewModel FolderList { get; }

        public NoteListViewModel(Api api, UIHost uiHost, FolderListViewModel folderList) : base(api, uiHost)
        {
            FolderList = folderList;

            ToggleFlagCommand = new AsyncRelayCommand<NoteItemViewModel>(ToggleFlag);
        }

        public async Task Load()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var folderId = FolderList.SelectedItem?.Entity.Id;
                if (folderId != null)
                {
                    var notes = await Api.NoteQuery.GetAll((Guid)folderId);
                    Items = new ObservableCollection<NoteItemViewModel>(notes.Select(CreateItem));
                }
                else
                {
                    Items = new ObservableCollection<NoteItemViewModel>();
                }
            });
        }


        protected override NoteItemViewModel CreateItem(NoteModel entity)
        {
            return new NoteItemViewModel(entity, this);
        }

        protected override EditorViewModel<NoteModel> CreateEditor(NoteItemViewModel item)
        {
            return new NoteEditorViewModel(Api, UIHost.ProgressService, FolderList, item?.Entity);
        }

        protected override bool CanContain(NoteModel entity)
        {
            return entity.FolderId == FolderList.SelectedItem?.Entity.Id;
        }


        private async Task ToggleFlag(NoteItemViewModel item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                item.Entity.IsFlagged = !item.Entity.IsFlagged;
                await Api.CommandBus.Execute(new SetNoteFlagCommand(item.Entity.Id, item.Entity.IsFlagged));
            });
        }

        protected override async Task Delete(NoteItemViewModel item)
        {
            await Api.CommandBus.Execute(new DeleteNoteCommand(item.Entity.Id));
        }

        protected override bool CanDelete(NoteItemViewModel item)
        {
            return true;
        }
    }
}
