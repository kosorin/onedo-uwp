using OneDo.Common.Logging;
using OneDo.Common.UI;
using OneDo.Model.Args;
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
                    UpdateDirtyProperty(() => SelectedFolder?.Entity.Id != Original.FolderId);
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
                    UpdateDirtyProperty(() => IsFlagged != Original.IsFlagged);
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
                    UpdateDirtyProperty(() => Title != Original.Title);
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
                    UpdateDirtyProperty(() => Text != Original.Text);
                }
            }
        }



        public DatePickerViewModel DatePicker { get; }

        public TimePickerViewModel ReminderPicker { get; }


        public DataService DataService { get; }

        public NoteBusiness Business { get; }

        public DateTimeBusiness DateTimeBusiness { get; }

        public NoteEditorViewModel(DataService dataService, IProgressService progressService, FolderListViewModel folderList)
            : this(dataService, progressService, folderList, null)
        {

        }

        public NoteEditorViewModel(DataService dataService, IProgressService progressService, FolderListViewModel folderList, Note note)
            : base(progressService)
        {
            DataService = dataService;
            Business = new NoteBusiness(DataService);
            DateTimeBusiness = new DateTimeBusiness(DataService);
            Original = note ?? Business.CreateDefault();

            Folders = folderList.Items.ToList();
            SelectedFolder = folderList.SelectedItem;

            DatePicker = new DatePickerViewModel(DataService);
            DatePicker.DateChanged += (s, e) =>
            {
                UpdateDirtyProperty(() => e.Date?.Date != Original.Date?.Date);
            };
            ReminderPicker = new TimePickerViewModel(DataService);
            ReminderPicker.TimeChanged += (s, e) =>
            {
                UpdateDirtyProperty(() => e.Time != Original.Reminder);
            };

            Load();
        }


        private void Load()
        {
            IsNew = DataService.Notes.IsNew(Original);

            if (!IsNew)
            {
                var folder = Folders.Where(x => x.Entity.Id == Original.FolderId).FirstOrDefault();
                if (folder != null)
                {
                    SelectedFolder = folder;
                }
            }
            IsFlagged = Original.IsFlagged;
            Title = Original.Title;
            Text = Original.Text;
            DatePicker.Date = Original.Date;
            ReminderPicker.Time = Original.Reminder;
        }

        protected override async Task Save()
        {
            Original.FolderId = SelectedFolder.Entity.Id;
            Original.IsFlagged = IsFlagged ?? false;
            Original.Title = Title ?? "";
            Original.Text = Text ?? "";
            Original.Date = DatePicker?.Date;
            Original.Reminder = ReminderPicker?.Time;

            if (IsNew)
            {
                Original.Created = DateTime.Now;
            }
            Original.Modified = DateTime.Now;

            await ProgressService.RunAsync(async () =>
            {
                await DataService.Notes.AddOrUpdate(Original);
            });
            OnSaved();
        }

        protected override async Task Delete()
        {
            await ProgressService.RunAsync(async () =>
            {
                await DataService.Notes.Delete(Original);
            });
            OnDeleted();
        }
    }
}