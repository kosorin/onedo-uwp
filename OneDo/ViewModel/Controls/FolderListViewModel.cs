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

namespace OneDo.ViewModel.Controls
{
    public class FolderListViewModel : ExtendedViewModel
    {
        private ObservableCollection<FolderItemObject> items;
        public ObservableCollection<FolderItemObject> Items
        {
            get { return items; }
            set { Set(ref items, value); }
        }

        private FolderItemObject selectedItem;
        public FolderItemObject SelectedItem
        {
            get { return selectedItem; }
            set { Set(ref selectedItem, value); }
        }

        public IModalService ModalService { get; }

        public ISettingsProvider SettingsProvider { get; }

        public IProgressService ProgressService { get; }

        public FolderListViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService)
        {
            ModalService = modalService;
            SettingsProvider = settingsProvider;
            ProgressService = progressService;
        }

        public async Task Load()
        {
            using (var dc = new DataContext())
            {
                if (!await dc.Set<Folder>().AnyAsync())
                {
                    dc.Set<Folder>().Add(new Folder { Name = "Inbox", Color = "#0063AF", });
                    dc.Set<Folder>().Add(new Folder { Name = "Work", Color = "#0F893E", });
                    dc.Set<Folder>().Add(new Folder { Name = "Shopping list", Color = "#AC008C", });
                    dc.Set<Folder>().Add(new Folder { Name = "Vacation", Color = "#F7630D", });
                    //dc.Set<Folder>().Add(new Folder { Name = "Folder with very long name", Color = "#00B6C1", });
                    await dc.SaveChangesAsync();
                }
                var folders = await dc.Set<Folder>().ToListAsync();
                var folderItems = folders.Select(f => new FolderItemObject(f));
                Items = new ObservableCollection<FolderItemObject>(folderItems);
                SelectedItem = Items.FirstOrDefault();
            }
        }

        public void AddItem()
        {
            var editor = new FolderEditorViewModel(ModalService, SettingsProvider, ProgressService);
            editor.Saved += (s, e) =>
            {
                Items.Add(new FolderItemObject(e.Entity));
                SelectedItem = Items.Last();
            };

            ShowEditor(editor);
        }

        public void EditItem(FolderItemObject item)
        {
            var editor = new FolderEditorViewModel(ModalService, SettingsProvider, ProgressService, item.Entity);
            editor.Deleted += (s, e) => Items.Remove(item);
            editor.Saved += (s, e) => item.Refresh();

            ShowEditor(editor);
        }

        public async Task DeleteItem(FolderItemObject item)
        {
            using (var dc = new DataContext())
            {
                if (SelectedItem == item)
                {
                    SelectedItem = Items.FirstOrDefault();
                }
                Items.Remove(item);
                dc.Set<Folder>().Attach(item.Entity);
                dc.Set<Folder>().Remove(item.Entity);
                await dc.SaveChangesAsync();
            }
        }

        private void ShowEditor(FolderEditorViewModel editor)
        {
            editor.Deleted += (s, e) => ModalService.CloseCurrent();
            editor.Saved += (s, e) => ModalService.CloseCurrent();
            ModalService.Show(editor);
        }
    }
}
