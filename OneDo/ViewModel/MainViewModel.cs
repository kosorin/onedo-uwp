using OneDo.Services.ModalService;
using Microsoft.EntityFrameworkCore;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using OneDo.ViewModel.Items;
using OneDo.ViewModel.Modals;
using Windows.UI.Core;
using OneDo.Model.Data.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OneDo.ViewModel.Commands;
using OneDo.Services.ProgressService;
using System;

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


        private ObservableCollection<NoteItemObject> noteItems;
        public ObservableCollection<NoteItemObject> NoteItems
        {
            get { return noteItems; }
            set { Set(ref noteItems, value); }
        }

        private NoteItemObject selectedNoteItem;
        public NoteItemObject SelectedNoteItem
        {
            get { return selectedNoteItem; }
            set { Set(ref selectedNoteItem, value); }
        }


        public ICommand AddNoteCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        public IModalService ModalService { get; }

        public ISettingsProvider SettingsProvider { get; }

        public IProgressService ProgressService { get; }

        public MainViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService)
        {
            ModalService = modalService;
            SettingsProvider = settingsProvider;
            ProgressService = progressService;

            FolderList = new FolderListViewModel(ModalService, SettingsProvider, ProgressService);
            NoteList = new NoteListViewModel(ModalService, SettingsProvider, ProgressService, FolderList);

            AddNoteCommand = new RelayCommand(NoteList.AddItem);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            FolderList.SelectionChanged += async (s, e) =>
            {
                if (e.Entity != null)
                {
                    await NoteList.Load(e.Entity.Id);
                }
                else
                {
                    NoteList.Clear();
                }
            };

            Load();
        }

        private async Task Load()
        {
            await FolderList.Load();
        }

        private async Task Clear()
        {
            await ProgressService.RunAsync(async () =>
            {
                using (var dc = new DataContext())
                {
                    await dc.Clear();
                    await dc.SaveChangesAsync();
                }
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
            ModalService.Show(new SettingsViewModel(ModalService, SettingsProvider));
        }
    }
}
