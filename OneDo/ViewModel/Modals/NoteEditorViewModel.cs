using GalaSoft.MvvmLight.Command;
using OneDo.Common.Event;
using OneDo.Common.Logging;
using OneDo.Model.Business;
using OneDo.Model.Business.Validation;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Commands;
using OneDo.ViewModel.Items;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModel.Modals
{
    public class NoteEditorViewModel : EditorViewModel<Note>
    {
        public List<FolderItemObject> Folders { get; }

        private FolderItemObject selectedFolder;
        public FolderItemObject SelectedFolder
        {
            get { return selectedFolder; }
            set
            {
                if (Set(ref selectedFolder, value))
                {
                    UpdateDirtyProperty(() => SelectedFolder?.Entity.Id != original.FolderId);
                }
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                if (Set(ref title, value))
                {
                    UpdateDirtyProperty(() => Title != original.Title);
                }
            }
        }

        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                if (Set(ref text, value))
                {
                    UpdateDirtyProperty(() => Text != original.Text);
                }
            }
        }

        private DateTimeOffset? date;
        public DateTimeOffset? Date
        {
            get { return date; }
            set
            {
                if (Set(ref date, value?.Date))
                {
                    UpdateDirtyProperty(() => Date != original.Date);
                }
            }
        }


        public event TypedEventHandler<NoteEditorViewModel, EntityEventArgs<Note>> Deleted;

        public event TypedEventHandler<NoteEditorViewModel, EntityEventArgs<Note>> Saved;


        public ICommand CompleteCommand { get; }

        private readonly NoteBusiness business;

        private readonly Note original;

        public NoteEditorViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService, FolderListViewModel folderList, Note note)
            : base(modalService, settingsProvider, progressService)
        {
            business = new NoteBusiness(SettingsProvider);
            original = note ?? business.Default();

            Folders = folderList.Items.ToList();
            SelectedFolder = folderList.SelectedItem;

            CompleteCommand = new AsyncRelayCommand(Complete);

            Load();
        }


        private void Load()
        {
            IsNew = business.IsNew(original);

            if (!IsNew)
            {
                var folder = Folders.Where(x => x.Entity.Id == original.FolderId).FirstOrDefault();
                if (folder != null)
                {
                    SelectedFolder = folder;
                }
            }
            Title = original.Title;
            Text = original.Text;
            Date = original.Date;
        }

        protected override async Task Delete()
        {
            if (!IsNew)
            {
                await ProgressService.RunAsync(async () =>
                {
                    using (var dc = new DataContext())
                    {
                        dc.Notes.Attach(original);
                        dc.Notes.Remove(original);
                        await dc.SaveChangesAsync();
                    }
                });

                OnDeleted();
            }
        }

        private async Task Complete()
        {
            business.ToggleComplete(original);
            await Save();
        }

        protected override async Task Save()
        {
            original.FolderId = SelectedFolder.Entity.Id;
            original.Title = Title;
            original.Text = Text;
            original.Date = Date?.DateTime;

            await ProgressService.RunAsync(async () =>
            {
                using (var dc = new DataContext())
                {
                    if (IsNew)
                    {
                        dc.Notes.Add(original);
                    }
                    else
                    {
                        dc.Notes.Update(original);
                    }
                    await dc.SaveChangesAsync();
                }
            });

            OnSaved();
        }

        protected override void OnSaved()
        {
            Saved?.Invoke(this, new EntityEventArgs<Note>(original));
        }

        protected override void OnDeleted()
        {
            Deleted?.Invoke(this, new EntityEventArgs<Note>(original));
        }
    }
}