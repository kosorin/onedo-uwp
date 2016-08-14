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

namespace OneDo.ViewModel.Controls
{
    public class FolderListViewModel : ExtendedViewModel
    {
        private ObservableCollection<FolderItemViewModel> folderItems;
        public ObservableCollection<FolderItemViewModel> FolderItems
        {
            get { return folderItems; }
            set { Set(ref folderItems, value); }
        }

        private FolderItemViewModel selectedFolderItem;
        public FolderItemViewModel SelectedFolderItem
        {
            get { return selectedFolderItem; }
            set { Set(ref selectedFolderItem, value); }
        }

        public ICommand AddCommand { get; }

        public ICommand EditCommand { get; }

        public ICommand DeleteCommand { get; }

        public FolderListViewModel()
        {

        }

        public async Task Load()
        {
            using (var dc = new DataContext())
            {
                if (await dc.Set<Folder>().FirstOrDefaultAsync() == null)
                {
                    dc.Set<Folder>().Add(new Folder
                    {
                        Name = "Inbox",
                        Color = "#0063AF",
                    });
                    await dc.SaveChangesAsync();
                }
                var folders = await dc.Set<Folder>().ToListAsync();
                var folderItems = folders.Select(f => new FolderItemViewModel(f));
                FolderItems = new ObservableCollection<FolderItemViewModel>(folderItems);
                SelectedFolderItem = FolderItems.FirstOrDefault();
            }
        }
    }
}
