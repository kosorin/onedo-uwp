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
    public class NoteListViewModel : ExtendedViewModel, INoteCommands
    {
        private ObservableCollection<NoteItemObject> items;
        public ObservableCollection<NoteItemObject> Items
        {
            get { return items; }
            set { Set(ref items, value); }
        }

        private NoteItemObject selectedItem;
        public NoteItemObject SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (Set(ref selectedItem, value))
                {
                    SelectionChanged?.Invoke(this, new EntityEventArgs<Note>(SelectedItem?.Entity));
                }
            }
        }

        public event TypedEventHandler<NoteListViewModel, EntityEventArgs<Note>> SelectionChanged;


        public IExtendedCommand AddCommand { get; }

        public IExtendedCommand EditCommand { get; }

        public IExtendedCommand DeleteCommand { get; }

        public IExtendedCommand ToggleFlagCommand { get; }


        public DataService DataService { get; }

        public UIHost UIHost { get; }

        public FolderListViewModel FolderList { get; }

        private readonly DateTimeBusiness dateTimeBusiness;

        public NoteListViewModel(DataService dataService, UIHost uiHost, FolderListViewModel folderList)
        {
            DataService = dataService;
            UIHost = uiHost;
            FolderList = folderList;
            dateTimeBusiness = new DateTimeBusiness(DataService);

            AddCommand = new RelayCommand(ShowNoteEditor);
            EditCommand = new RelayCommand<NoteItemObject>(ShowNoteEditor);
            DeleteCommand = new AsyncRelayCommand<NoteItemObject>(Delete);
            ToggleFlagCommand = new AsyncRelayCommand<NoteItemObject>(ToggleFlag);

            DataService.Notes.Saved += OnNoteSaved;
            DataService.Notes.Deleted += OnNoteDeleted;
        }

        public async Task Load(int folderId)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                var notes = await DataService
                    .Notes
                    .GetAll(x => x.FolderId == folderId);
                var noteItems = notes.Select(x => new NoteItemObject(x, dateTimeBusiness, this));
                Items = new ObservableCollection<NoteItemObject>(noteItems);
            });
        }

        public void Clear()
        {
            Items?.Clear();
        }

        public void Add(Note entity)
        {
            if (Items == null)
            {
                return;
            }

            if (CanContain(entity))
            {
                var item = GetItem(entity);
                if (item != null)
                {
                    item.Refresh();
                }
                else
                {
                    item = new NoteItemObject(entity, dateTimeBusiness, this);
                    Items.Add(item);
                }
            }
        }

        public void Remove(Note entity)
        {
            if (Items == null)
            {
                return;
            }

            var item = GetItem(entity);
            if (item != null)
            {
                Items.Remove(item);
            }
        }


        private void ShowNoteEditor()
        {
            var editor = new NoteEditorViewModel(DataService, UIHost.ProgressService, FolderList);
            editor.Saved += (s, e) =>
            {
                Add(e.Entity);
            };
            ShowNoteEditor(editor);
        }

        private void ShowNoteEditor(NoteItemObject item)
        {
            var editor = new NoteEditorViewModel(DataService, UIHost.ProgressService, FolderList, item.Entity);
            editor.Saved += (s, e) =>
            {
                if (e.Entity.FolderId == FolderList.SelectedItem?.Entity.Id)
                {
                    item.Refresh();
                }
                else
                {
                    FolderList.SelectedItem = FolderList.Items.FirstOrDefault(x => x.Entity.Id == item.Entity.FolderId);
                }
            };
            ShowNoteEditor(editor);
        }

        private void ShowNoteEditor(NoteEditorViewModel editor)
        {
            editor.Saved += (s, e) => UIHost.ModalService.Close();
            editor.Deleted += (s, e) => UIHost.ModalService.Close();
            UIHost.ModalService.Show(editor);
        }


        private async Task Delete(NoteItemObject item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await DataService.Notes.Delete(item.Entity);
            });
        }

        private async Task ToggleFlag(NoteItemObject item)
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                item.Entity.IsFlagged = !item.Entity.IsFlagged;
                item.Refresh();
                await DataService.Notes.Save(item.Entity);
            });
        }


        private void OnNoteSaved(object sender, EntityEventArgs<Note> e)
        {
            Add(e.Entity);
        }

        private void OnNoteDeleted(object sender, EntityEventArgs<Note> e)
        {
            Remove(e.Entity);
        }


        private bool CanContain(Note entity)
        {
            return entity.FolderId == FolderList.SelectedItem.Entity.Id;
        }

        private NoteItemObject GetItem(Note entity)
        {
            return Items.FirstOrDefault(x => x.Entity.Id == entity.Id);
        }
    }
}
