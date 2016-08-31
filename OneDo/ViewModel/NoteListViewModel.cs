using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using OneDo.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.ViewModel.Commands;
using OneDo.Services.ModalService;
using OneDo.ViewModel.Modals;
using OneDo.Services.ProgressService;
using Windows.Foundation;
using OneDo.Common.Event;

namespace OneDo.ViewModel
{
    public class NoteListViewModel : ExtendedViewModel
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

        public IModalService ModalService { get; }

        public ISettingsProvider SettingsProvider { get; }

        public IProgressService ProgressService { get; }

        public FolderListViewModel FolderList { get; }

        public NoteListViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService, FolderListViewModel folderList)
        {
            ModalService = modalService;
            SettingsProvider = settingsProvider;
            ProgressService = progressService;
            FolderList = folderList;
        }

        public async Task Load(int folderId)
        {
            await ProgressService.RunAsync(async () =>
            {
                using (var dc = new DataContext())
                {
                    var notes = await dc
                        .Notes
                        .Where(x => x.FolderId == folderId)
                        .ToListAsync();
                    var noteItems = notes.Select(x => new NoteItemObject(x));
                    Items = new ObservableCollection<NoteItemObject>(noteItems);
                }
            });
        }

        public void Clear()
        {
            Items?.Clear();
        }

        public void AddItem()
        {
            var editor = new NoteEditorViewModel(ModalService, SettingsProvider, ProgressService, FolderList, null);
            editor.Saved += (s, e) =>
            {
                if (e.Entity.FolderId == FolderList.SelectedItem?.Entity.Id)
                {
                    Items.Add(new NoteItemObject(e.Entity));
                }
            };
            ShowEditor(editor);
        }

        public void EditItem(NoteItemObject item)
        {
            var editor = new NoteEditorViewModel(ModalService, SettingsProvider, ProgressService, FolderList, item.Entity);
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

        private void ShowEditor(NoteEditorViewModel editor)
        {
            editor.Deleted += (s, e) => ModalService.CloseCurrent();
            editor.Saved += (s, e) => ModalService.CloseCurrent();
            ModalService.Show(editor);
        }
    }
}
