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

        public MainViewModel(DataService dataService, UIHost uiHost)
        {
            DataService = dataService;
            UIHost = uiHost;

            FolderList = new FolderListViewModel(DataService, UIHost);
            NoteList = new NoteListViewModel(DataService, UIHost, FolderList);

            FolderList.SelectionChanged += OnFolderSelectionChanged;

            DataService.Folders.Deleted += OnFolderDeleted;
            DataService.Notes.Deleted += OnNoteDeleted;

            ShowSettingsCommand = new RelayCommand(ShowSettings);


#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Load();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private async Task Load()
        {
            await FolderList.Load();
        }

        private async Task Clear()
        {
            await UIHost.ProgressService.RunAsync(async () =>
            {
                await DataService.Notes.DeleteAll();
                await DataService.Folders.DeleteAll();
            });
        }

#if DEBUG
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


        private async void OnFolderSelectionChanged(FolderListViewModel sender, EntityEventArgs<Folder> args)
        {
            if (args.Entity != null)
            {
                await NoteList.Load(args.Entity.Id);
            }
            else
            {
                NoteList.Clear();
            }
        }

        private void OnFolderDeleted(object sender, EntityEventArgs<Folder> e)
        {
            UIHost.InfoService.Hide();
        }

        private void OnNoteDeleted(object sender, EntityEventArgs<Note> e)
        {
            UIHost.InfoService.Show($"Deleted", InfoMessageDurations.Delete, InfoMessageColors.Default, InfoActionGlyphs.Undo, "Undo", async () =>
            {
                await DataService.Notes.SaveAsNew(e.Entity);
                NoteList.AddOrRefresh(e.Entity);
            });
        }
    }
}
