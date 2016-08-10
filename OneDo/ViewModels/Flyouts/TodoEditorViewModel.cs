using GalaSoft.MvvmLight.Command;
using OneDo.Common.Event;
using OneDo.Common.Logging;
using OneDo.Model.Business;
using OneDo.Model.Business.Validation;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using OneDo.Services.NavigationService;
using OneDo.ViewModels.Commands;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace OneDo.ViewModels.Flyouts
{
    public class TodoEditorViewModel : FlyoutViewModel
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
                if (Set(ref date, value))
                {
                    IsDirty = true;
                }
            }
        }


        public event EventHandler<TodoEditorViewModel, TodoEventArgs> Deleted;

        private void OnDeleted()
        {
            Deleted?.Invoke(this, new TodoEventArgs(original));
        }


        public event EventHandler<TodoEditorViewModel, TodoEventArgs> Saved;

        private void OnSaved()
        {
            Saved?.Invoke(this, new TodoEventArgs(original));
        }


        public ICommand DeleteCommand { get; }

        public ICommand DoneCommand { get; }

        public ICommand SaveCommand { get; }


        private readonly Todo original;

        private readonly TodoBusiness business;

        public TodoEditorViewModel(INavigationService navigationService, ISettingsProvider settingsProvider, Todo todo) : base(navigationService, settingsProvider)
        {
            business = new TodoBusiness(SettingsProvider);
            original = todo ?? business.GetDefault();


            DeleteCommand = new AsyncRelayCommand(Delete);
            DoneCommand = new AsyncRelayCommand(Done);
            SaveCommand = new AsyncRelayCommand(Save);

            Load();
        }


        private void Load()
        {
            IsNew = business.IsNew(original);

            Title = original.Title;
            Note = original.Note;
            Date = original.Date;

            IsDirty = IsNew;
        }

        private async Task Delete()
        {
            if (!IsNew)
            {
                using (var dc = new OneDoContext())
                {
                    dc.Set<Todo>().Attach(original);
                    dc.Set<Todo>().Remove(original);
                    await dc.SaveChangesAsync();
                }

                OnDeleted();
            }
        }

        private async Task Done()
        {
            original.Completed = DateTime.Now;
            await Save();
        }

        private async Task Save()
        {
            original.Title = Title;
            original.Note = Note;
            original.Date = Date?.DateTime;

            using (var dc = new OneDoContext())
            {
                if (IsNew)
                {
                    original.Id = Guid.NewGuid();
                    dc.Set<Todo>().Add(original);
                }
                else
                {
                    dc.Set<Todo>().Update(original);
                }
                await dc.SaveChangesAsync();
            }

            OnSaved();
        }
    }
}