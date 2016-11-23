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


        public IExtendedCommand AddCommand { get; }

        public IExtendedCommand EditCommand { get; }

        public IExtendedCommand DeleteCommand { get; }


        public IModalService ModalService { get; }

        public DataService DataService { get; }

        public IProgressService ProgressService { get; }

        public FolderListViewModel(IModalService modalService, DataService dataService, IProgressService progressService)
        {
            ModalService = modalService;
            DataService = dataService;
            ProgressService = progressService;

            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand<FolderItemObject>(Edit);
            DeleteCommand = new AsyncRelayCommand<FolderItemObject>(Delete, CanDelete);
        }

        public async Task Load()
        {
            await ProgressService.RunAsync(async () =>
            {
                var folders = await DataService.Folders.GetAll();

#if DEBUG
                if (!folders.Any())
                {
                    await DataService.Folders.Save(new Folder { Name = "Inbox", Color = "#0063AF", });
                    await DataService.Folders.Save(new Folder { Name = "Work", Color = "#0F893E", });
                    await DataService.Folders.Save(new Folder { Name = "Shopping list", Color = "#AC008C", });
                    await DataService.Folders.Save(new Folder { Name = "Vacation", Color = "#F7630D", });
                    folders = await DataService.Folders.GetAll();
                }

                if (!await DataService.Notes.Any())
                {
                    var folder = folders.FirstOrDefault();
                    var folder2 = folders.Skip(1).FirstOrDefault();
                    await DataService.Notes.Save(new Note
                    {
                        Title = "Buy milk",
                        Text = "",
                        FolderId = folder.Id,
                    });
                    await DataService.Notes.Save(new Note
                    {
                        Title = "Walk Max with bike",
                        Text = "",
                        Date = DateTime.Today,
                        Reminder = TimeSpan.FromHours(7.25),
                        FolderId = folder.Id,
                    });
                    await DataService.Notes.Save(new Note
                    {
                        Title = "Call mom",
                        Text = "",
                        Date = DateTime.Today.AddDays(5),
                        IsFlagged = true,
                        FolderId = folder.Id,
                    });
                    await DataService.Notes.Save(new Note
                    {
                        Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
                        Text = "Proin et diam at lorem egestas ullamcorper. Curabitur non eleifend mi. Praesent eu sem elementum, rutrum neque id, sollicitudin dolor. Proin molestie ullamcorper sem a hendrerit. Integer ac sapien erat. Morbi vehicula venenatis dolor, non aliquet nibh mattis sed.",
                        FolderId = folder.Id,
                    });
                    await DataService.Notes.Save(new Note
                    {
                        Title = "Test note",
                        Text = "",
                        IsFlagged = true,
                        FolderId = (folder2 ?? folder).Id,
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

        public async Task MoveItem(FolderItemObject folder, NoteItemObject note)
        {
            await ProgressService.RunAsync(async () =>
            {
                note.Entity.FolderId = folder.Entity.Id;
                await DataService.Notes.Save(note.Entity);
            });
            SelectedItem = folder;
        }

        private void Add()
        {
            var editor = new FolderEditorViewModel(DataService, ProgressService);
            editor.Saved += (s, e) =>
            {
                Items.Add(new FolderItemObject(e.Entity, this));
                SelectedItem = Items.Last();
            };

            ShowEditor(editor);
        }

        private void Edit(FolderItemObject item)
        {
            var editor = new FolderEditorViewModel(DataService, ProgressService, item.Entity);
            editor.Deleted += (s, e) => Items.Remove(item);
            editor.Saved += (s, e) => item.Refresh();

            ShowEditor(editor);
        }

        private bool CanDelete(FolderItemObject item)
        {
            return Items?.Count > 1;
        }

        private async Task Delete(FolderItemObject item)
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
            editor.Saved += (s, e) => ModalService.Close();
            editor.Deleted += (s, e) => ModalService.Close();
            ModalService.Show(editor);
        }
    }
}
