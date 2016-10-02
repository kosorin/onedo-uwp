using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using OneDo.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OneDo.ViewModel.Commands;
using OneDo.Services.ModalService;
using OneDo.ViewModel.Modals;
using OneDo.Services.ProgressService;
using Windows.Foundation;
using OneDo.Common.Event;
using OneDo.Common.UI;

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


        public IModalService ModalService { get; }

        public DataService DataService { get; }

        public IProgressService ProgressService { get; }

        public FolderListViewModel FolderList { get; }

        public NoteListViewModel(IModalService modalService, DataService dataService, IProgressService progressService, FolderListViewModel folderList)
        {
            ModalService = modalService;
            DataService = dataService;
            ProgressService = progressService;
            FolderList = folderList;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand<NoteItemObject>(Edit);
            DeleteCommand = new AsyncRelayCommand<NoteItemObject>(Delete);
        }

        public async Task Load(int folderId)
        {
            await ProgressService.RunAsync(async () =>
            {
                var notes = await DataService
                    .Notes
                    .GetAll(x => x.FolderId == folderId);
                var noteItems = notes.Select(x => new NoteItemObject(x, this));
                Items = new ObservableCollection<NoteItemObject>(noteItems);
            });
        }

        public void Clear()
        {
            Items?.Clear();
        }

        private void Add()
        {
            var editor = new NoteEditorViewModel(ModalService, DataService, ProgressService, FolderList, null);
            editor.Saved += (s, e) =>
            {
                if (e.Entity.FolderId == FolderList.SelectedItem?.Entity.Id)
                {
                    Items.Add(new NoteItemObject(e.Entity, this));
                }
            };
            ShowEditor(editor);
        }

        private void Edit(NoteItemObject item)
        {
            var editor = new NoteEditorViewModel(ModalService, DataService, ProgressService, FolderList, item.Entity);
            editor.Deleted += (s, e) => Items.Remove(item);
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

            ShowEditor(editor);
        }

        private async Task Delete(NoteItemObject item)
        {
            Items.Remove(item);
            if (SelectedItem == null)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            await DataService.Notes.Delete(item.Entity);
        }

        private void ShowEditor(NoteEditorViewModel editor)
        {
            ModalService.Show(editor);
        }
    }
}
