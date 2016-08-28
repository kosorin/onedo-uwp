﻿using OneDo.Services.ModalService;
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
            FolderList.SelectionChanged += async (s, e) => await LoadNotes(e.Entity.Id);
            FolderList.Load();
        }

        private async Task LoadNotes(int folderId)
        {
            await ProgressService.RunAsync(async () =>
            {
                using (var dc = new DataContext())
                {
                    var notes = await dc
                        .Set<Note>()
                        .Where(x => x.FolderId == folderId)
                        .ToListAsync();
                    var noteItems = notes.Select(t => new NoteItemObject(t));
                    NoteItems = new ObservableCollection<NoteItemObject>(noteItems);
                }
            });
        }

        private async Task ResetData()
        {
            await ProgressService.RunAsync(async () =>
            {
                using (var dc = new DataContext())
                {
                    var notes = await dc.Set<Note>().ToListAsync();
                    dc.Set<Note>().RemoveRange(notes);
                    await dc.SaveChangesAsync();
                    NoteItems.Clear();
                }
            });
        }

        private void AddNote()
        {
            var editor = new NoteEditorViewModel(ModalService, SettingsProvider, ProgressService, FolderList, null);
            editor.Saved += (s, e) =>
            {
                if (e.Entity.FolderId == FolderList.SelectedItem.Entity.Id)
                {
                    NoteItems.Add(new NoteItemObject(e.Entity));
                }
            };
            ShowNoteEditor(editor);
        }

        private void NoteItemTapped()
        {
            if (SelectedNoteItem != null)
            {
                var editor = new NoteEditorViewModel(ModalService, SettingsProvider, ProgressService, FolderList, SelectedNoteItem.Entity);
                editor.Deleted += (s, e) => NoteItems.Remove(SelectedNoteItem);
                editor.Saved += (s, e) => SelectedNoteItem.Refresh();
                ShowNoteEditor(editor);
            }
        }

        private void ShowNoteEditor(NoteEditorViewModel editor)
        {
            editor.Deleted += (s, e) => ModalService.CloseCurrent();
            editor.Saved += (s, e) => ModalService.CloseCurrent();
            ModalService.Show(editor);
        }

        private void ShowSettings()
        {
            ModalService.Show(new SettingsViewModel(ModalService, SettingsProvider));
        }
    }
}
