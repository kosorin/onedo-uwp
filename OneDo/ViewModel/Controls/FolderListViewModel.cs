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

namespace OneDo.ViewModel.Controls
{
    public class FolderListViewModel : ExtendedViewModel
    {
        private ObservableCollection<FolderItemViewModel> items;
        public ObservableCollection<FolderItemViewModel> Items
        {
            get { return items; }
            set { Set(ref items, value); }
        }

        private FolderItemViewModel selectedItem;
        public FolderItemViewModel SelectedItem
        {
            get { return selectedItem; }
            set { Set(ref selectedItem, value); }
        }

        public ICommand DeleteCommand { get; }

        public FolderListViewModel()
        {

        }

        public async Task Load()
        {
            using (var dc = new DataContext())
            {
                if (!await dc.Set<Folder>().AnyAsync())
                {
                    dc.Set<Folder>().Add(new Folder { Name = "Inbox", Color = "#0063AF", });
                    dc.Set<Folder>().Add(new Folder { Name = "Work", Color = "#0F893E", });
                    dc.Set<Folder>().Add(new Folder { Name = "Folder with very long name", Color = "#F7630D", });
                    dc.Set<Folder>().Add(new Folder { Name = "Shopping list", Color = "#AC008C", });
                    dc.Set<Folder>().Add(new Folder { Name = "Vacation", Color = "#00B6C1", });
                    await dc.SaveChangesAsync();
                }
                var folders = await dc.Set<Folder>().ToListAsync();
                var folderItems = folders.Select(f => new FolderItemViewModel(f));
                Items = new ObservableCollection<FolderItemViewModel>(folderItems);
                SelectedItem = Items.FirstOrDefault();
            }
        }

        private async Task Add(FolderItemViewModel item)
        {
            using (var dc = new DataContext())
            {
                dc.Set<Folder>().Add(item.Folder);
                await dc.SaveChangesAsync();
            }
        }
    }
}
