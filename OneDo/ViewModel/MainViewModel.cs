using OneDo.Services.ModalService;
using System.Windows.Input;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using OneDo.Model.Data.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OneDo.ViewModel.Commands;
using OneDo.Services.ProgressService;
using System;
using Microsoft.Band;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Band.Tiles;
using OneDo.Band;
using Microsoft.Band.Tiles.Pages;
using OneDo.Common.Logging;
using OneDo.Services.InfoService;
using OneDo.Model.Business;
using OneDo.Model.Args;
using OneDo.Services.ToastService;
using Windows.UI.Notifications;

namespace OneDo.ViewModel
{
    public class MainViewModel : ExtendedViewModel
    {
        private FolderListViewModel folderList;
        public FolderListViewModel FolderList
        {
            get { return folderList; }
            set { Set(ref folderList, value); }
        }

        private NoteListViewModel noteList;
        public NoteListViewModel NoteList
        {
            get { return noteList; }
            set { Set(ref noteList, value); }
        }


        public ICommand ShowSettingsCommand { get; }

        public DataService DataService { get; }

        public UIHost UIHost { get; }

        public IToastService ToastService { get; }

        public MainViewModel(DataService dataService, UIHost uiHost, IToastService toastService)
        {
            DataService = dataService;
            UIHost = uiHost;
            ToastService = toastService;

            FolderList = new FolderListViewModel(DataService, UIHost);
            NoteList = new NoteListViewModel(DataService, UIHost, FolderList);

            FolderList.SelectionChanged += OnFolderSelectionChanged;

            DataService.Folders.Deleted += OnFolderDeleted;
            DataService.Notes.Deleted += OnNoteDeleted;

            ShowSettingsCommand = new RelayCommand(ShowSettings);
        }

        public async Task Load()
        {
            await FolderList.Load();

#if DEBUG
            var folders = await DataService.Folders.GetAll();
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
                var folder2 = folders.Skip(1).FirstOrDefault();
                await DataService.Notes.Add(new Note
                {
                    Title = "Buy milk",
                    Text = "",
                    FolderId = folder.Id,
                });
                await DataService.Notes.Add(new Note
                {
                    Title = "Walk Max with bike",
                    Text = "",
                    Date = DateTime.Today,
                    Reminder = TimeSpan.FromHours(7.25),
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
                    Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
                    Text = "Proin et diam at lorem egestas ullamcorper. Curabitur non eleifend mi. Praesent eu sem elementum, rutrum neque id, sollicitudin dolor. Proin molestie ullamcorper sem a hendrerit. Integer ac sapien erat. Morbi vehicula venenatis dolor, non aliquet nibh mattis sed.",
                    FolderId = folder.Id,
                });
                await DataService.Notes.Add(new Note
                {
                    Title = "Test note",
                    Text = "",
                    IsFlagged = true,
                    FolderId = (folder2 ?? folder).Id,
                });
            }

            FolderList.SelectedItem = FolderList.Items.FirstOrDefault();
#endif
        }

#if DEBUG
        private async Task Clear()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await DataService.Folders.DeleteAll();
            });
        }

        public async Task ResetData()
        {
            await Clear();
            await Load();
        }
#endif

        private void ShowSettings()
        {
            UIHost.ModalService.Show(new SettingsViewModel(DataService));
        }


        private async void OnFolderSelectionChanged(object sender, EntityEventArgs<Folder> args)
        {
            if (args.Entity != null)
            {
                await NoteList.Load();
            }
            else
            {
                NoteList.Clear();
            }
        }

        private void OnFolderDeleted(object sender, EntityEventArgs<Folder> e)
        {
        }

        private void OnNoteDeleted(object sender, EntityEventArgs<Note> e)
        {
            UIHost.InfoService.Show($"Deleted", InfoMessageDurations.Delete, InfoMessageColors.Default, InfoActionGlyphs.Undo, "Undo", async () =>
            {
                await DataService.Notes.Add(e.Entity);
            });
        }
    }
}
