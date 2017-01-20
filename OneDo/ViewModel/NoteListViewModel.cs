using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OneDo.Common.Mvvm;
using OneDo.Application.Queries.Notes;
using OneDo.Application;
using System;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Queries.Folders;
using OneDo.ViewModel.Args;

namespace OneDo.ViewModel
{
    public class NoteListViewModel : ListViewModel<NoteItemViewModel, NoteModel>, INoteCommands
    {
        public IExtendedCommand ToggleFlagCommand { get; }


        public FolderListViewModel FolderList { get; set; }

        public NoteListViewModel(Api api, UIHost uiHost, FolderListViewModel folderList) : base(api, uiHost)
        {
            ToggleFlagCommand = new AsyncRelayCommand<NoteItemViewModel>(ToggleFlag);
            FolderList = folderList;
            FolderList.SelectionChanged += OnFolderListSelectionChanged;
        }

        private async void OnFolderListSelectionChanged(object sender, EntityEventArgs<FolderModel> args)
        {
            if (args.Entity != null)
            {
                await Load(args.Entity.Id);
            }
            else
            {
                Items.Clear();
            }
        }

        public async Task Load(Guid folderId)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var notes = await Api.NoteQuery.GetAll(folderId);
                Items = new ObservableCollection<NoteItemViewModel>(notes.Select(CreateItem));
            });
        }


        protected override NoteItemViewModel CreateItem(NoteModel entity)
        {
            return new NoteItemViewModel(entity, this);
        }

        protected override EditorViewModel<NoteModel> CreateEditor(NoteItemViewModel item)
        {
            return new NoteEditorViewModel(Api, UIHost.ProgressService, FolderList);
        }


        protected override async Task Delete(NoteItemViewModel item)
        {
            await Api.CommandBus.Execute(new DeleteNoteCommand(item.Entity.Id));
        }

        protected override bool CanDelete(NoteItemViewModel item)
        {
            return true;
        }


        private async Task ToggleFlag(NoteItemViewModel item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                item.Entity.IsFlagged = !item.Entity.IsFlagged;
                await Api.CommandBus.Execute(new SetNoteFlagCommand(item.Entity.Id, item.Entity.IsFlagged));
            });
        }
    }
}
