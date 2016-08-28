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
    public class NoteEditorViewModel : ModalViewModel
    {
        private bool isNew;
        public bool IsNew
        {
            get { return isNew; }
            set { Set(ref isNew, value); }
        }

        private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set { Set(ref isDirty, value); }
        }


        public List<FolderItemObject> Folders { get; }

        private FolderItemObject selectedFolder;
        public FolderItemObject SelectedFolder
        {
            get { return selectedFolder; }
            set
            {
                if (Set(ref selectedFolder, value))
                {
                    IsDirty = true;
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
                    IsDirty = true;
                }
            }
        }

        private string note;
        public string Note
        {
            get { return note; }
            set
            {
                if (Set(ref note, value))
                {
                    IsDirty = true;
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
                    IsDirty = true;
                }
            }
        }


        public event TypedEventHandler<NoteEditorViewModel, EntityEventArgs<Note>> Deleted;

        private void OnDeleted()
        {
            Deleted?.Invoke(this, new EntityEventArgs<Note>(original));
        }


        public event TypedEventHandler<NoteEditorViewModel, EntityEventArgs<Note>> Saved;

        private void OnSaved()
        {
            Saved?.Invoke(this, new EntityEventArgs<Note>(original));
        }


        public ICommand DeleteCommand { get; }

        public ICommand CompleteCommand { get; }

        public ICommand SaveCommand { get; }

        public IProgressService ProgressService { get; }

        private readonly Note original;

        private readonly NoteBusiness business;

        public NoteEditorViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService, FolderListViewModel folderList, Note note)
            : base(modalService, settingsProvider)
        {
            ProgressService = progressService;

            Folders = folderList.Items.ToList();
            RaisePropertyChanged(nameof(Folders));
            SelectedFolder = folderList.SelectedItem;

            business = new NoteBusiness(SettingsProvider);
            original = note ?? business.Default();

            DeleteCommand = new AsyncRelayCommand(Delete);
            CompleteCommand = new AsyncRelayCommand(Complete);
            SaveCommand = new AsyncRelayCommand(Save);

            Load();
        }


        private void Load()
        {
            IsNew = business.IsNew(original);

            if (IsNew)
            {
                original.FolderId = SelectedFolder.Entity.Id;
            }
            else
            {
                SelectedFolder = Folders.Where(x => x.Entity.Id == original.FolderId).FirstOrDefault() ?? Folders.FirstOrDefault();
            }
            Title = original.Title;
            Note = original.Text;
            Date = original.Date;

            IsDirty = IsNew;
        }

        private async Task Delete()
        {
            if (!IsNew)
            {
                await ProgressService.RunAsync(async () =>
                {
                    using (var dc = new DataContext())
                    {
                        dc.Set<Note>().Attach(original);
                        dc.Set<Note>().Remove(original);
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

        private async Task Save()
        {
            original.Title = Title;
            original.Text = Note;
            original.Date = Date?.DateTime;

            await ProgressService.RunAsync(async () =>
            {
                using (var dc = new DataContext())
                {
                    if (IsNew)
                    {
                        dc.Set<Note>().Add(original);
                    }
                    else
                    {
                        dc.Set<Note>().Update(original);
                    }
                    await dc.SaveChangesAsync();
                }
            });

            OnSaved();
        }
    }
}