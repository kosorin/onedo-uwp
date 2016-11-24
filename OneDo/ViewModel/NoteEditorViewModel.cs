using OneDo.Common.Event;
using OneDo.Common.Logging;
using OneDo.Common.UI;
using OneDo.Model.Business;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModel
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

        private bool? isFlagged;
        public bool? IsFlagged
        {
            get { return isFlagged; }
            set
            {
                if (Set(ref isFlagged, value))
                {
                    UpdateDirtyProperty(() => IsFlagged != original.IsFlagged);
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
                    ValidateProperty(() => !string.IsNullOrWhiteSpace(Title));
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



        public DatePickerViewModel DatePicker { get; }

        public TimePickerViewModel ReminderPicker { get; }


        public event TypedEventHandler<NoteEditorViewModel, EntityEventArgs<Note>> Deleted;

        public event TypedEventHandler<NoteEditorViewModel, EntityEventArgs<Note>> Saved;


        public DataService DataService { get; }

        private readonly DateTimeBusiness dateTimeBusiness;

        private readonly Note original;

        public NoteEditorViewModel(DataService dataService, IProgressService progressService, FolderListViewModel folderList)
            : this(dataService, progressService, folderList, null)
        {

        }

        public NoteEditorViewModel(DataService dataService, IProgressService progressService, FolderListViewModel folderList, Note note)
            : base(progressService)
        {
            DataService = dataService;
            dateTimeBusiness = new DateTimeBusiness(DataService);
            original = note ?? DataService.Notes.CreateDefault();

            Folders = folderList.Items.ToList();
            SelectedFolder = folderList.SelectedItem;

            DatePicker = new DatePickerViewModel(DataService);
            DatePicker.DateChanged += (s, e) =>
            {
                UpdateDirtyProperty(() => e.Date?.Date != original.Date?.Date);
            };
            ReminderPicker = new TimePickerViewModel(DataService);
            ReminderPicker.TimeChanged += (s, e) =>
            {
                UpdateDirtyProperty(() => e.Time != original.Reminder);
            };

            Load();
        }


        private void Load()
        {
            IsNew = DataService.Notes.IsNew(original);

            if (!IsNew)
            {
                var folder = Folders.Where(x => x.Entity.Id == original.FolderId).FirstOrDefault();
                if (folder != null)
                {
                    SelectedFolder = folder;
                }
            }
            IsFlagged = original.IsFlagged;
            Title = original.Title;
            Text = original.Text;
            DatePicker.Date = original.Date;
            ReminderPicker.Time = original.Reminder;
        }

        protected override Task Delete()
        {
            if (!IsNew)
            {
                var confirmation = new ConfirmationViewModel
                {
                    Text = "To-do will be deleted. Do you want to continue?",
                    ButtonText = "Yes, delete to-do",
                };
                confirmation.ConfirmationRequested += async (s, e) =>
                {
                    await ProgressService.RunAsync(async () =>
                    {
                        await DataService.Notes.Delete(original);
                    });
                    SubModalService.Close();
                    OnDeleted();
                };
                SubModalService.Show(confirmation);
            }
            return Task.CompletedTask;
        }

        protected override async Task Save()
        {
            original.FolderId = SelectedFolder.Entity.Id;
            original.IsFlagged = IsFlagged ?? false;
            original.Title = Title ?? "";
            original.Text = Text ?? "";
            original.Date = DatePicker?.Date;
            original.Reminder = ReminderPicker?.Time;

            await ProgressService.RunAsync(async () =>
            {
                await DataService.Notes.Save(original);
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