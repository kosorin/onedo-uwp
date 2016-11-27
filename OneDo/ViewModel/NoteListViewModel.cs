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
using OneDo.Services.InfoService;

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
