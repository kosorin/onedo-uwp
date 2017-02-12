using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using OneDo.Common.Mvvm;
using OneDo.Application.Queries.Notes;
using OneDo.Application;
using System;
using OneDo.Application.Commands.Notes;
using OneDo.Application.Queries.Folders;
using OneDo.Core;
using GalaSoft.MvvmLight.Messaging;
using OneDo.Core.CommandMessages;
using OneDo.Core.Args;
using OneDo.Common.Extensions;
using OneDo.Application.Events;
using OneDo.Application.Events.Notes;
using OneDo.Application.Models;

namespace OneDo.ViewModel
{
    public class NoteListViewModel : ListViewModel<NoteItemViewModel, NoteModel>
    {
        public FolderListViewModel FolderList { get; set; }

        public NoteListViewModel(IApi api, UIHost uiHost, FolderListViewModel folderList) : base(api, uiHost)
        {
            FolderList = folderList;
            FolderList.SelectionChanged += FolderList_SelectionChanged;

            Api.EventBus.Subscribe<NoteAddedEvent>(Note_Added, x => x.Model.FolderId == FolderList.SelectedItem?.Id);
            Api.EventBus.Subscribe<NoteUpdatedEvent>(Note_Updated, x => x.Model.FolderId != FolderList.SelectedItem?.Id);
            Api.EventBus.Subscribe<NoteMovedToFolderEvent>(Note_MovedToFolder, x => x.FolderId != FolderList.SelectedItem?.Id);
            Api.EventBus.Subscribe<NoteDeletedEvent>(Note_Deleted);
        }

        private async void FolderList_SelectionChanged(object sender, SelectionChangedEventArgs<FolderItemViewModel> args)
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
                Items = notes.Select(CreateItem).ToObservableCollection();
            });
        }

        private NoteItemViewModel CreateItem(NoteModel model)
        {
            return new NoteItemViewModel(model, Api, UIHost);
        }


        protected override void ShowEditor()
        {
            Messenger.Default.Send(new ShowNoteEditorMessage(null));
        }


        private void Note_Added(NoteAddedEvent @event)
        {
            Items.Add(CreateItem(@event.Model));
        }

        private void Note_Updated(NoteUpdatedEvent @event)
        {
            Items.Remove(Items.FirstOrDefault(x => x.Id == @event.Model.Id));
        }

        private void Note_MovedToFolder(NoteMovedToFolderEvent @event)
        {
            Items.Remove(Items.FirstOrDefault(x => x.Id == @event.Id));
        }

        private void Note_Deleted(NoteDeletedEvent @event)
        {
            Items.Remove(Items.FirstOrDefault(x => x.Id == @event.Id));
        }
    }
}
