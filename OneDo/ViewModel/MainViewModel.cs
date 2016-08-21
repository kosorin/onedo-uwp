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
using OneDo.ViewModel.Controls;

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


        private ObservableCollection<NoteItemViewModel> noteItems;
        public ObservableCollection<NoteItemViewModel> NoteItems
        {
            get { return noteItems; }
            set { Set(ref noteItems, value); }
        }

        private NoteItemViewModel selectedNoteItem;
        public NoteItemViewModel SelectedNoteItem
        {
            get { return selectedNoteItem; }
            set { Set(ref selectedNoteItem, value); }
        }


        public ICommand NoteItemTappedCommand { get; }

        public ICommand AddNoteCommand { get; }

        public ICommand ResetDataCommand { get; }

        public ICommand ShowSettingsCommand { get; }

        public IModalService ModalService { get; }

        public ISettingsProvider SettingsProvider { get; }

        public IProgressService ProgressService { get; }

        public MainViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService)
        {
            ModalService = modalService;
            SettingsProvider = settingsProvider;
            ProgressService = progressService;

            NoteItemTappedCommand = new RelayCommand(NoteItemTapped);
            AddNoteCommand = new RelayCommand(AddNote);
            ResetDataCommand = new AsyncRelayCommand(ResetData);
            ShowSettingsCommand = new RelayCommand(ShowSettings);

            FolderList = new FolderListViewModel(ModalService, SettingsProvider, ProgressService);

            LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                ProgressService.IsBusy = true;
                await FolderList.Load();
                using (var dc = new DataContext())
                {
                    if (await dc.Set<Note>().FirstOrDefaultAsync() == null)
                    {
                        dc.Set<Note>().Add(new Note
                        {
                            Title = "Buy milk",
                        });
                        dc.Set<Note>().Add(new Note
                        {
                            Title = "Call mom",
                            Date = DateTime.Today.AddDays(5),
                        });
                        dc.Set<Note>().Add(new Note
                        {
                            Title = "Walk Max",
                            Date = DateTime.Today,
                            Reminder = TimeSpan.FromHours(7.25),
                        });
                        await dc.SaveChangesAsync();
                    }
                    var notes = await dc.Set<Note>().ToListAsync();
                    var noteItems = notes.Select(t => new NoteItemViewModel(t));
                    NoteItems = new ObservableCollection<NoteItemViewModel>(noteItems);
                }
            }
            finally
            {
                ProgressService.IsBusy = false;
            }
        }

        private async Task ResetData()
        {
            try
            {
                ProgressService.IsBusy = true;
                using (var dc = new DataContext())
                {
                    var notes = NoteItems.Select(x => x.Entity);
                    dc.Set<Note>().AttachRange(notes);
                    dc.Set<Note>().RemoveRange(notes);
                    await dc.SaveChangesAsync();
                    NoteItems.Clear();
                }
            }
            finally
            {
                ProgressService.IsBusy = false;
            }
        }

        private void AddNote()
        {
            var editor = new NoteEditorViewModel(ModalService, SettingsProvider, ProgressService, null);
            editor.Saved += (s, e) => NoteItems.Add(new NoteItemViewModel(e.Entity));
            ShowNoteEditor(editor);
        }

        private void NoteItemTapped()
        {
            if (SelectedNoteItem != null)
            {
                var editor = new NoteEditorViewModel(ModalService, SettingsProvider, ProgressService, SelectedNoteItem.Entity);
                editor.Deleted += (s, e) => NoteItems.Remove(SelectedNoteItem);
                editor.Saved += (s, e) => SelectedNoteItem.Refresh();
                ShowNoteEditor(editor);
            }
        }

        private void ShowNoteEditor(NoteEditorViewModel editor)
        {
            editor.Deleted += (s, e) => ModalService.Pop();
            editor.Saved += (s, e) => ModalService.Pop();
            ModalService.Push(editor);
        }

        private void ShowSettings()
        {
            ModalService.Push(new SettingsViewModel(ModalService, SettingsProvider));
        }
    }
}
