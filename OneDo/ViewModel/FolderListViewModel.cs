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
using GalaSoft.MvvmLight.Command;
using OneDo.ViewModel.Commands;
using OneDo.Services.ModalService;
using OneDo.ViewModel.Modals;
using OneDo.Services.ProgressService;
using Windows.Foundation;
using OneDo.Common.Event;

namespace OneDo.ViewModel
{
    public class FolderListViewModel : ExtendedViewModel, IFolderCommands
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
            set
            {
                if (Set(ref selectedItem, value))
                {
                    SelectionChanged?.Invoke(this, new EntityEventArgs<Folder>(SelectedItem?.Entity));
                }
            }
        }

        public event TypedEventHandler<FolderListViewModel, EntityEventArgs<Folder>> SelectionChanged;


        public RelayCommand AddCommand { get; }

        public RelayCommand<FolderItemObject> EditCommand { get; }

        public AsyncRelayCommand<FolderItemObject> DeleteCommand { get; }


        public IModalService ModalService { get; }

        public DataService DataService { get; }

        public IProgressService ProgressService { get; }

        public FolderListViewModel(IModalService modalService, DataService dataService, IProgressService progressService)
        {
            ModalService = modalService;
            DataService = dataService;
            ProgressService = progressService;

            AddCommand = new RelayCommand(AddItem);
            EditCommand = new RelayCommand<FolderItemObject>(EditItem);
            DeleteCommand = new AsyncRelayCommand<FolderItemObject>(DeleteItem, CanDeleteItem);
        }

        public async Task Load()
        {
            await ProgressService.RunAsync(async () =>
            {
                var folders = await DataService.Folders.GetAll();

#if DEBUG
                if (!folders.Any())
                {
                    await DataService.Folders.Add(new Folder { Name = "Inbox", Color = "#0063AF", });
                    await DataService.Folders.Add(new Folder { Name = "Work", Color = "#0F893E", });
                    await DataService.Folders.Add(new Folder { Name = "Shopping list", Color = "#AC008C", });
                    await DataService.Folders.Add(new Folder { Name = "Vacation", Color = "#F7630D", });
                    folders = await DataService.Folders.GetAll();
                }

                if (!await DataService.Notes.Any())
                {
                    var folder = folders.FirstOrDefault();
                    await DataService.Notes.Add(new Note
                    {
                        Title = "Buy milk",
                        Text = "",
                        IsFlagged = false,
                        FolderId = folder.Id,
                    });
                    await DataService.Notes.Add(new Note
                    {
                        Title = "Call mom",
                        Text = "",
                        Date = DateTime.Today.AddDays(5),
                        IsFlagged = true,
                        FolderId = folder.Id,
                    });
                    await DataService.Notes.Add(new Note
                    {
                        Title = "Walk Max",
                        Text = "",
                        Date = DateTime.Today,
                        Reminder = TimeSpan.FromHours(7.25),
                        IsFlagged = false,
                        FolderId = folder.Id,
                    });
                }
#endif
                var folderItems = folders.Select(x => new FolderItemObject(x, this));
                Items = new ObservableCollection<FolderItemObject>(folderItems);
                Items.CollectionChanged += (s, e) =>
                {
                    DeleteCommand.RaiseCanExecuteChanged();
                };
                SelectedItem = Items.FirstOrDefault();
            });
        }

        private void AddItem()
        {
            var editor = new FolderEditorViewModel(ModalService, DataService, ProgressService);
            editor.Saved += (s, e) =>
            {
                Items.Add(new FolderItemObject(e.Entity, this));
                SelectedItem = Items.Last();
            };

            ShowEditor(editor);
        }

        private void EditItem(FolderItemObject item)
        {
            var editor = new FolderEditorViewModel(ModalService, DataService, ProgressService, item.Entity);
            editor.Deleted += (s, e) => Items.Remove(item);
            editor.Saved += (s, e) => item.Refresh();

            ShowEditor(editor);
        }

        private bool CanDeleteItem(FolderItemObject item)
        {
            return Items?.Count > 1;
        }

        private async Task DeleteItem(FolderItemObject item)
        {
            Items.Remove(item);
            if (SelectedItem == null)
            {
                SelectedItem = Items.FirstOrDefault();
            }
            await DataService.Folders.Delete(item.Entity);
        }

        private void ShowEditor(FolderEditorViewModel editor)
        {
            ModalService.Show(editor);
        }
    }
}
