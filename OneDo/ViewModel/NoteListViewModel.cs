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
using OneDo.ViewModel.Parameters;
using OneDo.ViewModel.Items;
using OneDo.Core;
using GalaSoft.MvvmLight.Messaging;

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

        private async void OnFolderListSelectionChanged(object sender, SelectionChangedEventArgs<FolderItemViewModel> args)
        {
            if (args.Item != null)
            {
                await Load(args.Item.Id);
            }
            else
            {
                Items.Clear();
            }
        }

        private async Task Load(Guid folderId)
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


        protected override void ShowEditor(NoteItemViewModel note)
        {
            Messenger.Default.Send(new ShowNoteEditorMessage(note?.Id));
        }


        protected override async Task Delete(NoteItemViewModel item)
        {
            await Api.CommandBus.Execute(new DeleteNoteCommand(item.Id));
        }

        protected override bool CanDelete(NoteItemViewModel item)
        {
            return true;
        }


        private async Task ToggleFlag(NoteItemViewModel item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await Api.CommandBus.Execute(new SetNoteFlagCommand(item.Id, !item.IsFlagged));
            });
        }
    }
}
