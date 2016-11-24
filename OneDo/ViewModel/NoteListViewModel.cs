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


        public IModalService ModalService { get; }

        public DataService DataService { get; }

        public IProgressService ProgressService { get; }

        public IInfoService InfoService { get; }

        public FolderListViewModel FolderList { get; }

        private readonly DateTimeBusiness dateTimeBusiness;

        public NoteListViewModel(IModalService modalService, DataService dataService, IProgressService progressService, IInfoService infoService, FolderListViewModel folderList)
        {
            ModalService = modalService;
            DataService = dataService;
            ProgressService = progressService;
            InfoService = infoService;
            FolderList = folderList;
            dateTimeBusiness = new DateTimeBusiness(DataService);

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand<NoteItemObject>(Edit);
            DeleteCommand = new AsyncRelayCommand<NoteItemObject>(Delete);
            ToggleFlagCommand = new AsyncRelayCommand<NoteItemObject>(ToggleFlag);

            DataService.Notes.Deleted += OnDeleted;
        }

        private void OnDeleted(object sender, EntityEventArgs<Note> e)
        {
            if (Items != null)
            {
                var entity = e.Entity;
                var item = Items.FirstOrDefault(x => x.Entity.Id == entity.Id);
                Items.Remove(item);
                if (SelectedItem == null)
                {
                    SelectedItem = Items.FirstOrDefault();
                }

                InfoService.Show($"Deleted", InfoMessageDurations.Delete, InfoMessageColors.Default, InfoActionGlyphs.Undo, "Undo", async () =>
                {
                    await DataService.Notes.SaveAsNew(entity);
                    Items.Add(new NoteItemObject(entity, dateTimeBusiness, this));
                });
            }
        }

        public async Task Load(int folderId)
        {
            await ProgressService.RunAsync(async () =>
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

        private void Add()
        {
            var editor = new NoteEditorViewModel(DataService, ProgressService, FolderList);
            editor.Saved += (s, e) =>
            {
                if (e.Entity.FolderId == FolderList.SelectedItem?.Entity.Id)
                {
                    Items.Add(new NoteItemObject(e.Entity, dateTimeBusiness, this));
                }
            };
            ShowNoteEditor(editor);
        }

        private void Edit(NoteItemObject item)
        {
            var editor = new NoteEditorViewModel(DataService, ProgressService, FolderList, item.Entity);
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

        private async Task Delete(NoteItemObject item)
        {
            await ProgressService.RunAsync(async () =>
            {
                await DataService.Notes.Delete(item.Entity);
            });
        }

        private void ShowNoteEditor(NoteEditorViewModel editor)
        {
            editor.Saved += (s, e) => ModalService.Close();
            editor.Deleted += (s, e) => ModalService.Close();
            ModalService.Show(editor);
        }

        private async Task ToggleFlag(NoteItemObject item)
        {
            await ProgressService.RunAsync(async () =>
            {
                item.Entity.IsFlagged = !item.Entity.IsFlagged;
                item.Refresh();
                await DataService.Notes.Save(item.Entity);
            });
        }
    }
}
